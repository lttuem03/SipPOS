/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function (knex) {
  // Deletes ALL existing entries in the 'products' table
  await knex('product').del();

  // Inserts seed entries
  await knex('product').insert([
    {
      id: 1,
      created_at: new Date(2022, 4, 15),
      created_by: "admin",
      name: "Coca Cola",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      desc: "Nước giải khát có ga, phổ biến trên toàn thế giới",
      price: 12.50,
      category_id: 1,
      status: "Available"
    },
    {
      id: 2,
      created_at: new Date(2021, 7, 22),
      created_by: "admin",
      name: "Pepsi",
      desc: "Nước ngọt có ga với hương vị truyền thống",
      price: 11.00,
      category_id: 1,
      status: "Available"
    },
    {
      id: 3,
      created_at: new Date(2020, 11, 5),
      created_by: "admin",
      name: "Sprite",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      desc: "Nước giải khát có ga, vị chanh tươi mát",
      price: 10.75,
      category_id: 1,
      status: "Unavailable"
    },
    {
      id: 4,
      created_at: new Date(2023, 2, 12),
      created_by: "admin",
      name: "Nước cam ép",
      desc: "Nước cam ép nguyên chất, giàu vitamin C",
      price: 15.00,
      category_id: 2,
      status: "Available"
    },
    {
      id: 5,
      created_at: new Date(2022, 6, 20),
      created_by: "admin",
      name: "Nước ép táo",
      desc: "Nước ép táo ngọt tự nhiên, giàu chất xơ",
      price: 14.00,
      category_id: 2,
      status: "Unavailable"
    },
    {
      id: 6,
      created_at: new Date(2021, 9, 15),
      created_by: "admin",
      name: "Nước ép dừa",
      desc: "Nước ép dừa tươi ngon, giàu chất khoáng",
      price: 16.00,
      category_id: 2,
      status: "Available"
    },
    {
      id: 7,
      created_at: new Date(2020, 10, 5),
      created_by: "admin",
      name: "Nước suối Lavie",
      desc: "Nước suối Lavie nguyên chất, không chất bảo quản",
      price: 5.00,
      category_id: 3,
      status: "Available"
    },
    {
      id: 8,
      created_at: new Date(2023, 2, 12),
      created_by: "admin",
      name: "Nước suối Aquafina",
      desc: "Nước suối Aquafina nguyên chất, không chất bảo quản",
      price: 6.00,
      category_id: 3,
      status: "Available"
    },
    {
      id: 9,
      created_at: new Date(2022, 6, 20),
      created_by: "admin",
      name: "Nước suối Dasani",
      desc: "Nước suối Dasani nguyên chất, không chất bảo quản",
      price: 7.00,
      category_id: 3,
      status: "Unavailable"
    },
    {
      id: 10,
      created_at: new Date(2021, 9, 15),
      created_by: "admin",
      name: "Red Bull",
      desc: "Nước tăng lực Red Bull, giúp tăng cường năng lượng",
      price: 8.00,
      category_id: 5,
      status: "Available"
    },
    {
      id: 11,
      created_at: new Date(2020, 10, 5),
      created_by: "admin",
      name: "Sting",
      desc: "Nước tăng lực Sting, giúp tăng cường năng lượng",
      price: 9.00,
      category_id: 5,
      status: "Available"
    }
  ]);
};
