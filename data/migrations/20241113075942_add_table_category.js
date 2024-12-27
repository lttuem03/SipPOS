/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function (knex) {
    await knex.raw(`
        CREATE TABLE category (
            id INT NOT NULL,
            store_id INT REFERENCES store(id) ON DELETE CASCADE NOT NULL,
            created_by VARCHAR(64),
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_by VARCHAR(64),
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            deleted_by VARCHAR(64),
            deleted_at TIMESTAMP,
            name VARCHAR(255) NOT NULL,
            description TEXT,

            PRIMARY KEY (store_id, id)
        );

        CREATE OR REPLACE FUNCTION reset_category_id() RETURNS TRIGGER AS $$
        BEGIN
            IF NEW.id IS NULL THEN
                NEW.id := COALESCE(
                    (SELECT MAX(id) + 1 FROM category WHERE store_id = NEW.store_id),
                    0
                );
            END IF;
            RETURN NEW;
        END;
        $$ LANGUAGE plpgsql;
            
        CREATE TRIGGER reset_category_id_trigger
        BEFORE INSERT ON category
        FOR EACH ROW
        EXECUTE FUNCTION reset_category_id();
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function (knex) {
    await knex.raw(`
        DROP TRIGGER IF EXISTS reset_category_id_trigger ON category;
        DROP FUNCTION IF EXISTS reset_category_id;
        DROP TABLE IF EXISTS category
    `);
};
