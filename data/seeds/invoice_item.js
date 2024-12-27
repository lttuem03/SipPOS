/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
    // Deletes ALL existing entries
    await knex('invoice_item').del();

    await knex('invoice_item').insert([
        // Đơn hàng 1, item 1, (6) Trà Sen Vàng (M) - 55.000
        {
            invoice_id: 0,
            store_id: 0,
            product_id: 6,
            item_name: "Trà Sen Vàng",
            option_name: "M",
            option_price: 55_000,
            discount: 0,
            note: "Ít đá"
        },
        // Đơn hàng 1, item 2, (20) Bánh Phô mai Trà Xanh (S) - 35.000
        {
            invoice_id: 0,
            store_id: 0,
            product_id: 20,
            item_name: "Bánh Phô mai Trà Xanh",
            option_name: "S",
            option_price: 35_000,
            discount: 0
        }
    ]);
};
