/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE invoice_item (
            id INT NOT NULL,
            invoice_id INT NOT NULL,
            store_id INT NOT NULL,
            product_id INT NOT NULL,
            item_name VARCHAR(64) NOT NULL,
            option_name VARCHAR(64) NOT NULL,
            option_price NUMERIC(12, 2) NOT NULL,
            discount NUMERIC(12, 2) NOT NULL,
            note VARCHAR(64) DEFAULT '' NOT NULL,

            PRIMARY KEY (store_id, invoice_id, id),
            FOREIGN KEY (store_id, invoice_id) REFERENCES invoice(store_id, id)
        );

        CREATE OR REPLACE FUNCTION reset_invoice_item_id() RETURNS TRIGGER AS $$
        BEGIN
            IF NEW.id IS NULL THEN
                NEW.id := COALESCE(
                    (
                        SELECT MAX(id) + 1 
                        FROM invoice_item 
                        WHERE 
                            store_id = NEW.store_id
                        AND
                            invoice_id = NEW.invoice_id
                    ),
                    0
                );
            END IF;
            RETURN NEW;
        END;
        $$ LANGUAGE plpgsql;
            
        CREATE TRIGGER reset_invoice_item_id_trigger
        BEFORE INSERT ON invoice_item
        FOR EACH ROW
        EXECUTE FUNCTION reset_invoice_item_id();
    `);
};


/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TRIGGER IF EXISTS reset_invoice_item_id_trigger ON invoice_item;
        DROP FUNCTION IF EXISTS reset_invoice_item_id;
        DROP TABLE IF EXISTS invoice_item
    `);
};