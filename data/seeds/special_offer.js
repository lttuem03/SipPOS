/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
    await knex('special_offer').del();

    await knex('special_offer').insert([
        {
            code: "TET20",
            store_id: 0,
            created_by: "SM000000",
            created_at: "2025-01-03 00:09:16.466516",
            updated_by: null,
            updated_at: "2025-01-03 00:09:16.471972",
            deleted_by: null,
            deleted_at: null,
            name: "Khuyến mãi của ngày tết",
            description: "TET",
            sold_items: 0.00,
            max_items: 1000.00,
            category_store_id: null,
            category_id: null,
            product_id: null,
            type: "InvoicePromotion",
            price_type: "Original",
            start_date: "2025-01-03 00:08:51.173152",
            end_date: "2025-01-11 00:08:51.184751",
            discount_price: 20000.00,
            discount_percentage: 0.00,
            status: "Inactive"
        },
        {
            code: "TET10%",
            store_id: 0,
            created_by: "SM000000",
            created_at: "2025-01-03 00:10:19.017869",
            updated_by: null,
            updated_at: "2025-01-03 00:10:19.018071",
            deleted_by: null,
            deleted_at: null,
            name: "Khuyến mãi tết",
            description: "TET",
            sold_items: 0.00,
            max_items: 1000.00,
            category_store_id: null,
            category_id: 1,
            product_id: null,
            type: "CategoryPromotion",
            price_type: "Percentage",
            start_date: "2025-01-03 00:09:41.517738",
            end_date: "2025-01-25 00:08:51.184751",
            discount_price: 1.00,
            discount_percentage: 10.00,
            status: "Active"
        }
    ]);
};
