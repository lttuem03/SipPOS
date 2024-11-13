/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function (knex) {
  // Deletes ALL existing entries in the 'categories' table
  await knex('category').del();

  // Inserts seed entries
  await knex('category').insert([
    {
      id: 1,
      name: "Nước giải khát",
      desc: "Đây là danh mục nước giải khát",
      status: "Available",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      created_at: new Date(2022, 4, 15), // Months are zero-indexed in JavaScript
      created_by: "admin"
    },
    {
      id: 2,
      name: "Nước trái cây",
      desc: "Danh mục các loại nước ép trái cây",
      status: "Available",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      created_at: new Date(2022, 4, 15),
      created_by: "admin"
    },
    {
      id: 3,
      name: "Nước suối",
      desc: "Danh mục các loại nước suối",
      status: "Available",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      created_at: new Date(2022, 4, 15),
      created_by: "admin"
    },
    {
      id: 4,
      name: "Nước ngọt",
      desc: "Danh mục các loại nước ngọt",
      status: "Available",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      created_at: new Date(2022, 4, 15),
      created_by: "admin"
    },
    {
      id: 5,
      name: "Nước tăng lực",
      desc: "Danh mục các loại nước tăng lực",
      status: "Available",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      created_at: new Date(2022, 4, 15),
      created_by: "admin"
    },
    {
      id: 6,
      name: "Nước ép",
      desc: "Danh mục các loại nước ép",
      status: "Available",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      created_at: new Date(2022, 4, 15),
      created_by: "admin"
    },
    {
      id: 7,
      name: "Nước lọc",
      desc: "Danh mục các loại nước lọc",
      status: "Available",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      created_at: new Date(2022, 4, 15),
      created_by: "admin"
    },
    {
      id: 8,
      name: "Nước đóng chai",
      desc: "Danh mục các loại nước đóng chai",
      status: "Available",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      created_at: new Date(2022, 4, 15),
      created_by: "admin"
    },
    {
      id: 9,
      name: "Nước đóng lon",
      desc: "Danh mục các loại nước đóng lon",
      status: "Available",
      created_at: new Date(2022, 4, 15),
      created_by: "admin"
    },
    {
      id: 10,
      name: "Nước đóng gói",
      desc: "Danh mục các loại nước đóng gói",
      status: "Available",
      created_at: new Date(2022, 4, 15),
      created_by: "admin"
    },
    {
      id: 11,
      name: "Nước đóng hộp",
      desc: "Danh mục các loại nước đóng hộp",
      status: "Available",
      image_urls: [
        "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
        "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
      ],
      created_at: new Date(2022, 4, 15),
      created_by: "admin"
    }
  ]);
};
