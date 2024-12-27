/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE product_option (
            id INT NOT NULL,
            product_id INT NOT NULL,
            store_id INT NOT NULL,
            name VARCHAR(64) NOT NULL DEFAULT '',
            price NUMERIC(12, 2) NOT NULL,

            PRIMARY KEY (store_id, product_id, id),
            FOREIGN KEY (store_id, product_id) REFERENCES product(store_id, id)
        );

        CREATE OR REPLACE FUNCTION reset_product_option_id() RETURNS TRIGGER AS $$
        BEGIN
            IF NEW.id IS NULL THEN
                NEW.id := COALESCE(
                    (
                        SELECT MAX(id) + 1 
                        FROM product_option 
                        WHERE store_id = NEW.store_id
                        AND product_id = NEW.product_id
                    ),
                    0
                );
            END IF;
            RETURN NEW;
        END;
        $$ LANGUAGE plpgsql;
            
        CREATE TRIGGER reset_product_option_id_trigger
        BEFORE INSERT ON product_option
        FOR EACH ROW
        EXECUTE FUNCTION reset_product_option_id();
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TRIGGER IF EXISTS reset_product_option_id_trigger ON product_option;
        DROP FUNCTION IF EXISTS reset_product_option_id;
        DROP TABLE IF EXISTS product_option
    `);
};
