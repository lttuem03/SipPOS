/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE shift (
            id SERIAL UNIQUE NOT NULL,
            store_id INT REFERENCES store(id) ON DELETE CASCADE NOT NULL,
            staff_store_id INT NOT NULL,
            staff_id INT NOT NULL,
            start_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
            end_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
        
            PRIMARY KEY (store_id, id),
            FOREIGN KEY (staff_store_id, staff_id) REFERENCES staff(store_id, id) ON DELETE CASCADE
        )   
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS shift
    `);
};
