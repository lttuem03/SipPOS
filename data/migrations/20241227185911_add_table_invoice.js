/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE invoice (
            id INT NOT NULL,
            store_id INT REFERENCES store(id) ON DELETE CASCADE NOT NULL,
            staff_id INT NOT NULL,
            staff_name VARCHAR(64) NOT NULL,
            created_at TIMESTAMP NOT NULL,
            item_count INT NOT NULL,
            sub_total NUMERIC(12, 2) NOT NULL,
            total_discount NUMERIC(12, 2) NOT NULL,
            invoice_based_vat NUMERIC(12, 2) NOT NULL,
            total NUMERIC(12, 2) NOT NULL,
            customer_paid NUMERIC(12, 2) NOT NULL,
            change NUMERIC(12, 2) NOT NULL,
            payment_method VARCHAR(24) NOT NULL 
                CHECK (payment_method IN ('CASH', 'QR_PAY')),

            PRIMARY KEY (store_id, id)
        );

        CREATE OR REPLACE FUNCTION reset_invoice_id() RETURNS TRIGGER AS $$
        BEGIN
            IF NEW.id IS NULL THEN
                NEW.id := COALESCE(
                    (
                        SELECT MAX(id) + 1 
                        FROM invoice 
                        WHERE store_id = NEW.store_id
                    ),
                    0
                );
            END IF;
            RETURN NEW;
        END;
        $$ LANGUAGE plpgsql;
            
        CREATE TRIGGER reset_invoice_id_trigger
        BEFORE INSERT ON invoice
        FOR EACH ROW
        EXECUTE FUNCTION reset_invoice_id();
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TRIGGER IF EXISTS reset_invoice_id_trigger ON invoice;
        DROP FUNCTION IF EXISTS reset_invoice_id;
        DROP TABLE IF EXISTS invoice
    `);
};