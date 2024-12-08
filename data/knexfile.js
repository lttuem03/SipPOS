// Update with your config settings.
const path_to_env = require('path').resolve(__dirname, '../.env');
require('dotenv').config({ path: path_to_env });

/**
 * @type { Object.<string, import("knex").Knex.Config> }
 */
module.exports = {
    development: {
        client: 'pg', // for postgres
        connection: {
            host: process.env.POSTGRES_HOST,
            port: parseInt(process.env.POSTGRES_PORT),
            user: process.env.POSTGRES_USERNAME,
            password: process.env.POSTGRES_PASSWORD,
            database: process.env.POSTGRES_DATABASE,
            timezone: 'Asia/Bangkok'
        },
        migrations: {
            directory: './migrations'
        },
        seeds: {
            directory: './seeds'
        },
        pool: {
            afterCreate: (conn, done) => {
                conn.query('SET timezone="Asia/Bangkok";', (err) => {
                    if (err) {
                        done(err, conn);
                    } else {
                        done(null, conn);
                    }
                });
            }
        }
    }
};
