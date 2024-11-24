/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE staff (
            id SERIAL UNIQUE NOT NULL,
            store_id SERIAL REFERENCES store(id) ON DELETE CASCADE NOT NULL,
            created_by VARCHAR(64),
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_by VARCHAR(64),
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            deleted_by VARCHAR(64),
            deleted_at TIMESTAMP,
            position_prefix VARCHAR(2) NOT NULL,
            password_hash VARCHAR(64) NOT NULL CHECK (LENGTH(password_hash) = 64),
            salt VARCHAR(64) NOT NULL CHECK (LENGTH(salt) = 64),
            name VARCHAR(64) NOT NULL,
            date_of_birth DATE NOT NULL,
            email VARCHAR(64) NOT NULL,
            tel VARCHAR(16) NOT NULL,
            address VARCHAR(255) NOT NULL,
            employment_status VARCHAR(24) NOT NULL,
            employment_start_date DATE NOT NULL,
            employment_end_date DATE,

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
        DROP TABLE IF EXISTS staff
    `);
};
