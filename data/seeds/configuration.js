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
            current_staff_base_salary: 0,
            current_staff_hourly_salary: 21000,
            current_assistant_manager_base_salary: 5000000,
            current_assistant_manager_hourly_salary: 12000,
            current_store_manager_base_salary: 7000000,
            current_store_manager_hourly_salary: 15000,
            next_staff_base_salary: 0,
            next_staff_hourly_salary: 21000,
            next_assistant_manager_base_salary: 5000000,
            next_assistant_manager_hourly_salary: 12000,
            next_store_manager_base_salary: 7000000,
            next_store_manager_hourly_salary: 15000
        }
    ]);
};
