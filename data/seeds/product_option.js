/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
    // Deletes ALL existing entries
    await knex('product_option').del();

    await knex('product_option').insert([
        // 0. Phin Sữa Đá
        { product_id: 0, store_id: 0, name: "S", price: 29_000 },
        { product_id: 0, store_id: 0, name: "M", price: 39_000 },
        { product_id: 0, store_id: 0, name: "L", price: 45_000 },
        // 1. Phin Đen Đá
        { product_id: 1, store_id: 0, name: "S", price: 29_000 },
        { product_id: 1, store_id: 0, name: "M", price: 35_000 },
        { product_id: 1, store_id: 0, name: "L", price: 39_000 },
        // 2. Bạc Xỉu
        { product_id: 2, store_id: 0, name: "S", price: 29_000 },
        { product_id: 2, store_id: 0, name: "M", price: 39_000 },
        { product_id: 2, store_id: 0, name: "L", price: 45_000 },
        // 3. PhinDi Hạnh Nhân
        { product_id: 3, store_id: 0, name: "S", price: 45_000 },
        { product_id: 3, store_id: 0, name: "M", price: 49_000 },
        { product_id: 3, store_id: 0, name: "L", price: 55_000 },
        // 4. PhinDi Kem Sữa
        { product_id: 4, store_id: 0, name: "S", price: 45_000 },
        { product_id: 4, store_id: 0, name: "M", price: 49_000 },
        { product_id: 4, store_id: 0, name: "L", price: 55_000 },
        // 5. PhinDi Choco
        { product_id: 5, store_id: 0, name: "S", price: 45_000 },
        { product_id: 5, store_id: 0, name: "M", price: 49_000 },
        { product_id: 5, store_id: 0, name: "L", price: 55_000 },
        // 6. Trà Sen Vàng
        { product_id: 6, store_id: 0, name: "S", price: 45_000 },
        { product_id: 6, store_id: 0, name: "M", price: 55_000 },
        { product_id: 6, store_id: 0, name: "L", price: 65_000 },
        // 7. Trà Thạch Đào
        { product_id: 7, store_id: 0, name: "S", price: 45_000 },
        { product_id: 7, store_id: 0, name: "M", price: 55_000 },
        { product_id: 7, store_id: 0, name: "L", price: 65_000 },
        // 8. Trà Thanh Đào
        { product_id: 8, store_id: 0, name: "S", price: 45_000 },
        { product_id: 8, store_id: 0, name: "M", price: 55_000 },
        { product_id: 8, store_id: 0, name: "L", price: 65_000 },
        // 9. Trà Thạch Vải
        { product_id: 9, store_id: 0, name: "S", price: 45_000 },
        { product_id: 9, store_id: 0, name: "M", price: 55_000 },
        { product_id: 9, store_id: 0, name: "L", price: 65_000 },
        // 10. Trà Xanh Đậu Đỏ
        { product_id: 10, store_id: 0, name: "S", price: 45_000 },
        { product_id: 10, store_id: 0, name: "M", price: 55_000 },
        { product_id: 10, store_id: 0, name: "L", price: 65_000 },
        // 11. Freeze Trà Xanh
        { product_id: 11, store_id: 0, name: "S", price: 55_000 },
        { product_id: 11, store_id: 0, name: "M", price: 65_000 },
        { product_id: 11, store_id: 0, name: "L", price: 69_000 },
        // 12. Caramel Phin Freeze
        { product_id: 12, store_id: 0, name: "S", price: 55_000 },
        { product_id: 12, store_id: 0, name: "M", price: 65_000 },
        { product_id: 12, store_id: 0, name: "L", price: 69_000 },
        // 13. Cookies & Cream
        { product_id: 13, store_id: 0, name: "S", price: 55_000 },
        { product_id: 13, store_id: 0, name: "M", price: 65_000 },
        { product_id: 13, store_id: 0, name: "L", price: 69_000 },
        // 14. Freeze Sô-cô-la
        { product_id: 14, store_id: 0, name: "S", price: 55_000 },
        { product_id: 14, store_id: 0, name: "M", price: 65_000 },
        { product_id: 14, store_id: 0, name: "L", price: 69_000 },
        // 15. Classic Phin Freeze
        { product_id: 15, store_id: 0, name: "S", price: 55_000 },
        { product_id: 15, store_id: 0, name: "M", price: 65_000 },
        { product_id: 15, store_id: 0, name: "L", price: 69_000 },
        // 16. Bánh Croissant
        { product_id: 16, store_id: 0, name: "S", price: 29_000 },
        // 17. Bánh Chuối
        { product_id: 17, store_id: 0, name: "S", price: 29_000 },
        // 18. Bánh Phô mai Cà phê
        { product_id: 18, store_id: 0, name: "S", price: 29_000 },
        // 19. Bánh Phô mai Chanh dây
        { product_id: 19, store_id: 0, name: "S", price: 29_000 },
        // 20. Bánh Phô mai Trà Xanh
        { product_id: 20, store_id: 0, name: "S", price: 35_000 },
        // 21. Bánh Caramel Phô mai
        { product_id: 21, store_id: 0, name: "S", price: 35_000 },
        // 22. Bánh Tiramisu
        { product_id: 22, store_id: 0, name: "S", price: 35_000 },
        // 23. Bánh Mousse Đào
        { product_id: 23, store_id: 0, name: "S", price: 35_000 },
        // 24. Bánh Mousse Cacao
        { product_id: 24, store_id: 0, name: "S", price: 35_000 },
    ]);
};
