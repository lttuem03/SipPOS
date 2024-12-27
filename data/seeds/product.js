/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function (knex) {
    // Deletes ALL existing entries in the 'products' table
    await knex('product').del();

    // Inserts seed entries
    await knex('product').insert([
        // Danh mục:
        //      0. Cà phê PHIN
        //      1. PHINDI
        //      2. Trà
        //      3. Freeze
        //      4. Bánh

        // 0. Phin Sữa Đá (danh mục 0. Cà phê PHIN)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Phin Sữa Đá",
            description: "Iced Coffee with Condensed Milk",
            category_store_id: 0,
            category_id: 0,
            image_uris: [
                
            ]
        },
        // 1. Phin Đen Đá (danh mục 0. Cà phê PHIN)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Phin Đen Đá",
            description: "Iced Black Coffee",
            category_store_id: 0,
            category_id: 0,
            image_uris: [
                
            ]
        },
        // 2. Bạc Xỉu (danh mục 0. Cà phê PHIN)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bạc Xỉu",
            description: "Iced White PHIN Coffee & Condensed Milk",
            category_store_id: 0,
            category_id: 0,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/04_2023/New_product/HLC_New_logo_5.1_Products__BAC_XIU.jpg"
            ]
        },
        // 3. PhinDi Hạnh Nhân (danh mục 1. PhinDi)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "PhinDi Hạnh Nhân",
            description: "Iced Coffee with Almond & Fresh Milk",
            category_store_id: 0,
            category_id: 1,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__PHINDI_HANH_NHAN.jpg"
            ]
        },
        // 4. PhinDi Kem Sữa (danh mục 1. PhinDi)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "PhinDi Kem Sữa",
            description: "Iced Coffee with Milk Foam",
            category_store_id: 0,
            category_id: 1,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__PHINDI_KEM_SUA.jpg"
            ]
        },
        // 5. PhinDi Choco (danh mục 1. PhinDi)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "PhinDi Choco",
            description: "Iced Coffee with Chocolate",
            category_store_id: 0,
            category_id: 1,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__PHINDI_CHOCO.jpg"
            ]
        },
        // 6. Trà Sen Vàng (danh mục 2. Trà)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Trà Sen Vàng",
            description: "Tea with Lotus Seeds",
            category_store_id: 0,
            category_id: 2,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__TSV.jpg"
            ]
        },
        // 7. Trà Thạch Đào (danh mục 2. Trà)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Trà Thạch Đào",
            description: "Tea with Peach Jelly",
            category_store_id: 0,
            category_id: 2,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__TRA_THANH_DAO-09.jpg"
            ]
        },
        // 8. Trà Thanh Đào (danh mục 2. Trà)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Trà Thanh Đào",
            description: "Peach Tea with Lemongrass",
            category_store_id: 0,
            category_id: 2,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__TRA_THANH_DAO-08.jpg"
            ]
        },
        // 9. Trà Thạch Vải (danh mục 2. Trà)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Trà Thạch Vải",
            description: "Tea with Lychee Jelly",
            category_store_id: 0,
            category_id: 2,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__TRA_TACH_VAI.jpg"
            ]
        },
        // 10. Trà Xanh Đậu Đỏ (danh mục 2. Trà)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Trà Xanh Đậu Đỏ",
            description: "Green Tea with Red Bean",
            category_store_id: 0,
            category_id: 2,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__TRA_XANH_DAU_DO.jpg"
            ]
        },
        // 11. Freeze Trà Xanh (danh mục 3. Freeze)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Freeze Trà Xanh",
            description: "Green Tea Freeze",
            category_store_id: 0,
            category_id: 3,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__FREEZE_TRA_XANH.jpg"
            ]
        },
        // 12. Caramel Phin Freeze (danh mục 3. Freeze)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Caramel Phin Freeze",
            description: "",
            category_store_id: 0,
            category_id: 3,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__CARAMEL_FREEZE_PHINDI.jpg"
            ]
        },
        // 13. Cookies & Cream (danh mục 3. Freeze)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Cookies & Cream",
            description: "",
            category_store_id: 0,
            category_id: 3,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__COOKIES_FREEZE.jpg"
            ]
        },
        // 14. Freeze Sô-cô-la (danh mục 3. Freeze)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Freeze Sô-cô-la",
            description: "Chocolate Freeze",
            category_store_id: 0,
            category_id: 3,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/04_2023/New_product/HLC_New_logo_5.1_Products__FREEZE_CHOCO.jpg"
            ]
        },
        // 15. Classic Phin Freeze (danh mục 3. Freeze)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Classic Phin Freeze",
            description: "",
            category_store_id: 0,
            category_id: 3,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__CLASSIC_FREEZE_PHINDI.jpg"
            ]
        },
        // 16. Bánh Croissant (danh mục 4. Bánh)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bánh Croissant",
            description: "",
            category_store_id: 0,
            category_id: 4,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/2024_Food/Croissant.png"
            ]
        },
        // 17. Bánh Chuối (danh mục 4. Bánh)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bánh Chuối",
            description: "",
            category_store_id: 0,
            category_id: 4,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/thumbs/Banh-Chuoi.png"
            ]
        },
        // 18. Bánh Phô mai Cà phê (danh mục 4. Bánh)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bánh Phô mai Cà phê",
            description: "",
            category_store_id: 0,
            category_id: 4,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/03_2018/PHOMAICAPHE.jpg"
            ]
        },
        // 19. Bánh Phô mai Chanh dây (danh mục 4. Bánh)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bánh Phô mai Chanh dây",
            description: "",
            category_store_id: 0,
            category_id: 4,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/03_2018/PHOMAICHANHDAY.jpg"
            ]
        },
        // 20. Bánh Phô mai Trà Xanh (danh mục 4. Bánh)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bánh Phô mai Trà Xanh",
            description: "",
            category_store_id: 0,
            category_id: 4,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/thumbs/Pho-Mai-Tra-Xanh.png"
            ]
        },
        // 21. Bánh Caramel Phô mai (danh mục 4. Bánh)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bánh Caramel Phô mai",
            description: "",
            category_store_id: 0,
            category_id: 4,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/03_2018/CARAMELPHOMAI.jpg"
            ]
        },
        // 22. Bánh Tiramisu (danh mục 4. Bánh)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bánh Tiramisu",
            description: "",
            category_store_id: 0,
            category_id: 4,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/thumbs/Tiramisu.png"
            ]
        },
        // 23. Bánh Mousse Đào (danh mục 4. Bánh)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bánh Mousse Đào",
            description: "",
            category_store_id: 0,
            category_id: 4,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/thumbs/Mousse-Dao.png"
            ]
        },
        // 24. Bánh Mousse Cacao (danh mục 4. Bánh)
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bánh Mousse Cacao",
            description: "",
            category_store_id: 0,
            category_id: 4,
            image_uris: [
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/thumbs/Mousse-Cacao.png"
            ]
        },
    ]);
};
