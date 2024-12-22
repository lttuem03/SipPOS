/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE store (
            id SERIAL PRIMARY KEY,
            created_by VARCHAR(64),
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_by VARCHAR(64),
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            deleted_by VARCHAR(64),
            deleted_at TIMESTAMP,
            name VARCHAR(64) NOT NULL,
            address VARCHAR(255) NOT NULL,
            email VARCHAR(64) NOT NULL,
            tel VARCHAR(16) NOT NULL,
            username VARCHAR(32) UNIQUE NOT NULL,
            password_hash VARCHAR(64) NOT NULL CHECK (LENGTH(password_hash) = 64),
            salt VARCHAR(64) NOT NULL CHECK (LENGTH(salt) = 64),
            last_login TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
        )
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS store
    `);
};