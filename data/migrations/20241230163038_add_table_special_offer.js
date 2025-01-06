/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE special_offer (
            id INT NOT NULL,
            code VARCHAR(50) NOT NULL,
            store_id INT REFERENCES store(id) ON DELETE CASCADE NOT NULL,
            created_by VARCHAR(64),
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_by VARCHAR(64),
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            deleted_by VARCHAR(64),
            deleted_at TIMESTAMP,
            name VARCHAR(64) NOT NULL,
            description TEXT DEFAULT '' NOT NULL,
            items_sold INT DEFAULT 0 NOT NULL,
            max_items INT DEFAULT 0 NOT NULL,
            category_store_id INT,
            category_id INT,
            product_id INT,
            type VARCHAR(50) NOT NULL CHECK (type IN ('ProductPromotion', 'CategoryPromotion', 'InvoicePromotion')),
            price_type VARCHAR(50),
            start_date TIMESTAMP,
            end_date TIMESTAMP,
            discount_price NUMERIC(12, 2),
            discount_percentage NUMERIC(12, 2),
            status VARCHAR(50) DEFAULT 'Inactive' NOT NULL,

            PRIMARY KEY (store_id, id)
        );

        CREATE OR REPLACE FUNCTION reset_special_offer_id() RETURNS TRIGGER AS $$
        BEGIN
            IF NEW.id IS NULL THEN
                NEW.id := COALESCE(
                    (SELECT MAX(id) + 1 FROM special_offer WHERE store_id = NEW.store_id),
                    0
                );
            END IF;
            RETURN NEW;
        END;
        $$ LANGUAGE plpgsql;
            
        CREATE TRIGGER reset_special_offer_id_trigger
        BEFORE INSERT ON special_offer
        FOR EACH ROW
        EXECUTE FUNCTION reset_special_offer_id();
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TRIGGER IF EXISTS reset_special_offer_id_trigger ON special_offer;
        DROP FUNCTION IF EXISTS reset_special_offer_id;
        DROP TABLE IF EXISTS special_offer
    `); 
};
