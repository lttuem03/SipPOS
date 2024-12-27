/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function (knex) {
    // Deletes ALL existing entries in the 'category' table
    await knex('category').del();

    // Inserts seed entries
    await knex('category').insert([
        // 0. Cà phê PHIN
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Cà phê PHIN",
            description: "Sự kết hợp hoàn hảo giữa hạt cà phê Robusta & Arabica thượng hạng được trồng trên những vùng cao nguyên Việt Nam."
        },
        // 1. PhinDi
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "PhinDi"
        },
        // 2. Trà
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Trà",
            description: "Hương vị tự nhiên, thơm ngon của Trà Việt với phong cách hiện đại sẽ giúp bạn gợi mở vị giác của bản thân và tận hưởng một cảm giác thật khoan khoái, tươi mới."
        },
        // 3. Freeze
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Freeze",
            description: "Thức uống đá xay mát lạnh được pha chế từ những nguyên liệu thuần túy của Việt Nam."
        },
        // 4. Bánh
        {
            store_id: 0,
            created_by: "admin",
            created_at: new Date(),
            name: "Bánh",
            description: "Sẽ càng ngon miệng hơn khi bạn kết hợp đồ uống với những chiếc bánh ngọt thơm ngon được làm thủ công hàng ngày ngay tại bếp bánh của chúng tôi."
        }
    ]);
};
