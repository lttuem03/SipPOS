/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE shift (
            id SERIAL UNIQUE NOT NULL,
            store_id SERIAL REFERENCES store(id) ON DELETE CASCADE NOT NULL,
            staff_id SERIAL REFERENCES staff(id) ON DELETE CASCADE NOT NULL,
            start_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
            end_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

            PRIMARY KEY (store_id, id)
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
