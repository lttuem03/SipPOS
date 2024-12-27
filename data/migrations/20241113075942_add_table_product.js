/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function (knex) {
    await knex.raw(`
        CREATE TABLE product (
            id INT NOT NULL,
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
            category_store_id INT,
            category_id INT,
            image_uris TEXT[],
            status VARCHAR(50) DEFAULT 'Available' NOT NULL,

            PRIMARY KEY (store_id, id),
            FOREIGN KEY (category_store_id, category_id) REFERENCES category(store_id, id)
        );

        CREATE OR REPLACE FUNCTION reset_product_id() RETURNS TRIGGER AS $$
        BEGIN
            IF NEW.id IS NULL THEN
                NEW.id := COALESCE(
                    (SELECT MAX(id) + 1 FROM product WHERE store_id = NEW.store_id),
                    0
                );
            END IF;
            RETURN NEW;
        END;
        $$ LANGUAGE plpgsql;
            
        CREATE TRIGGER reset_product_id_trigger
        BEFORE INSERT ON product
        FOR EACH ROW
        EXECUTE FUNCTION reset_product_id();
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function (knex) {
    await knex.raw(`
        DROP TRIGGER IF EXISTS reset_product_id_trigger ON product;
        DROP FUNCTION IF EXISTS reset_product_id;
        DROP TABLE IF EXISTS product
    `);
};
