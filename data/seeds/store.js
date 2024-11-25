/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('store').del()
  
  await knex('store').insert([
    {
        id: 0,
        created_by: 'system',
        name: "Mộc Ông Công Kính Quán",
        address: "123 Đường Mộc Oanh Cửa Khẩu, Phường 9, Quận Tân Bình, Thành phố Hồ Chí Minh",
        email: "canhkheoquan@gmail.com",
        tel: "09323654213",
        tax_code: "7200000010001",
        username: "mock",
        password_hash: "8B7E4258694259386AF545199FDBF898CA9127C0BCAA2E81449F706789AA62A3",
        salt: "C7FC0185B7C8F6A48C7E00AEE44B1EBF61CB60E4B4C4D62EE97BD07F4DD826DD"
    }
  ]);
};
