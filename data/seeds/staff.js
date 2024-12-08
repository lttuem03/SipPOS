/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('staff').del()
  await knex('staff').insert([
    // SM000000 - Cửa hàng trưởng Mai Thị Quỳnh - InEmployment
    {
        id: 0,
        store_id: 0,
        created_by: "system",
        position_prefix: "SM",
        password_hash: "8B7E4258694259386AF545199FDBF898CA9127C0BCAA2E81449F706789AA62A3",
        salt: "C7FC0185B7C8F6A48C7E00AEE44B1EBF61CB60E4B4C4D62EE97BD07F4DD826DD",
        name: "Mai Thị Quỳnh",
        gender: "Nữ",
        date_of_birth: "19990520",
        email: "quynhnguyen@gmail.com",
        tel: "0928512665",
        address: "123 Đường Nguyễn Thị Minh Khai, Phường 9, Quận Tân Bình, Thành phố Hồ Chí Minh",
        employment_status: "InEmployment",
        employment_start_date: "20210329"
    },
    // ST000001 - Nhân viên bán hàng Nguyễn Văn Đức - OutOfEmployment
    {
        id: 1,
        store_id: 0,
        created_by: "system",
        position_prefix: "ST",
        password_hash: "8B7E4258694259386AF545199FDBF898CA9127C0BCAA2E81449F706789AA62A3",
        salt: "C7FC0185B7C8F6A48C7E00AEE44B1EBF61CB60E4B4C4D62EE97BD07F4DD826DD",
        name: "Nguyễn Văn Đức",
        gender: "Nam",
        date_of_birth: "20020120",
        email: "vanduc@gmail.com",
        tel: "0903514265",
        address: "463 Đường Đồng Văn Cống, Phường 2, Quận Thủ Đức, Thành phố Hồ Chí Minh",
        employment_status: "OutOfEmployment",
        employment_start_date: "20231203",
        employment_end_date: "20240515"
    }
  ]);
};
