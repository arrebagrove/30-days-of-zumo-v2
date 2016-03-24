var azureMobileApps = require('azure-mobile-apps');

// Create a new table definition
var table = azureMobileApps.table();

// Require authentication
table.access = 'authenticated';

// CREATE operation
table.insert(function (context) {
  context.user.getIdentity().then(function (userInfo) {
    context.item.userId = userInfo.aad.claims.emailaddress;
    return context.execute();
  });
});

// READ operation
table.read(function (context) {
  context.user.getIdentity().then(function (userInfo) {
    context.query.where({ userId: userInfo.aad.claims.emailaddress });
    return context.execute();
  });
});

// UPDATE operation
table.update(function (context) {
  context.user.getIdentity().then(function (userInfo) {
    context.query.where({ userId: userInfo.aad.claims.emailaddress });
    context.item.userId = userInfo.aad.claims.emailaddress;
    return context.execute();
  });
});

// DELETE operation
table.delete(function (context) {
  context.user.getIdentity().then(function (userInfo) {
    context.query.where({ userId: userInfo.aad.claims.emailaddress });
    return context.execute();
  });
});

module.exports = table;
