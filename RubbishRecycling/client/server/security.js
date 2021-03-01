const helmet = require('helmet');
/** Using helmetJs to configure security headers, please refer to https://helmetjs.github.io/docs/
 * for more details about the default headers included.
 */
function configureHeaders(app) {
  app.use(
    helmet({
      frameguard: false // some clients use infotrack via iframe
    })
  );
  app.use(helmet.referrerPolicy({ policy: 'no-referrer-when-downgrade' }));
}

module.exports.configureHeaders = configureHeaders;
