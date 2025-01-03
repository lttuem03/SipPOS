/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
    // Deletes ALL existing entries
    await knex('invoice').del();

    await knex('invoice').insert([
        {
            store_id: 0, 
            staff_id: 0,
            staff_name: "Mai Thị Quỳnh",
            created_at: new Date(),
            item_count: 2,
            sub_total: 90_000,
            total_discount: 0,
            invoice_based_vat: 0,
            total: 90_000,
            customer_paid: 100_000,
            change: 10_000,
            payment_method: "CASH"
        },
    ]);
};
