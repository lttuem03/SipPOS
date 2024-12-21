/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
    await knex('configuration').del();

    await knex('configuration').insert([
        {
            store_id: 0,
            opening_time: "07:30:00",
            closing_time: "22:30:00",
            tax_code: "0100000011-001",
            vat_rate: 0.05,
            vat_method: "VAT_INCLUDED",
            staff_base_salary: 0,
            staff_hourly_salary: 21000,
            assistant_manager_base_salary: 5000000,
            assistant_manager_hourly_salary: 12000,
            store_manager_base_salary: 7000000,
            store_manager_hourly_salary: 15000
        }
    ]);
};
