const express = require('express');
const compression = require('compression');
const security = require('./security');
const cookieParser = require('cookie-parser');
const fs = require('fs');
const path = require('path');

const app = express();

const appName = 'InfoTrack.Cdd.Client';
const pathBase = ['/service/cdd/*', '/newwebsite/service/cdd/*'];
const version = fs.readFileSync(path.join(__dirname, 'BUILD_NUMBER.txt')).toString();

function checkMappingCookie(req, res, next) {
  const mappingId = req.cookies['MappingId'];
  if (mappingId) {
    res.cookie('MappingId', mappingId, { httpOnly: false });
  }
  next();
}

function nocache(req, res, next) {
  res.header('Cache-Control', 'private, no-cache, no-store, must-revalidate');
  res.header('Expires', '-1');
  res.header('Pragma', 'no-cache');
  next();
}

app.use(compression());
app.use(cookieParser());
security.configureHeaders(app);
app.use('/', express.static('dist', { maxAge: '20 days' }));
app.use(checkMappingCookie);

const versionRoute = `${pathBase}/version`;
app.get(versionRoute, nocache, (req, res) => {
  res.json({ version });
});
console.info('Registered version route:', versionRoute);

const healthCheckRoute = `${pathBase}/health*`;
app.get(healthCheckRoute, nocache, (req, res) => {
  res.json({
    appName,
    version,
    status: 'Healthy',
    uptime: process.uptime(),
    timestamp: new Date().toISOString()
  });
});
console.info('Registered health check route:', healthCheckRoute);

app.get('/*', nocache, (req, res) => {
  res.sendFile(path.join(__dirname, 'index.html'));
});

const port = 80;
app.listen(port, () => {
  console.info(`${appName} is listening on port ${port}`);
});
