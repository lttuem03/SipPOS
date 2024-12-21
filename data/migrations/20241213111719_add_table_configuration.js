/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE configuration (
            store_id INT REFERENCES store(id) ON DELETE CASCADE NOT NULL,
            opening_time TIME NOT NULL,
            closing_time TIME NOT NULL,
            tax_code VARCHAR(14) NOT NULL,
            vat_rate NUMERIC(7, 5) NOT NULL,
            vat_method VARCHAR(12) NOT NULL 
                CHECK (vat_method IN ('VAT_INCLUDED', 'ORDER_BASED')),
            staff_base_salary NUMERIC(12, 2) NOT NULL,
            staff_hourly_salary NUMERIC(12, 2) NOT NULL,
            assistant_manager_base_salary NUMERIC(12, 2) NOT NULL,
            assistant_manager_hourly_salary NUMERIC(12, 2) NOT NULL,
            store_manager_base_salary NUMERIC(12, 2) NOT NULL,
            store_manager_hourly_salary NUMERIC(12, 2) NOT NULL,

            PRIMARY KEY (store_id)
        )
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS configuration  
    `);
};
