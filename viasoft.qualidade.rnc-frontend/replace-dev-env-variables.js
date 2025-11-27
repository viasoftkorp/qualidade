const { readFileSync, writeFileSync } = require('fs');
const path = require('path');
const { glob } = require("glob");

var environmentTsFile = "";
var appsettingsFile = "";

let files = glob.sync(__dirname + '/apps/**/environment.dev-env.ts');
if(files.length == 0) {
  throw new Error("Unable to find 'environment.dev-env.ts'");
}
environmentTsFile = files[0];

files = glob.sync(__dirname + '/apps/**/appsettings.dev-env.json');
if(files.length == 0) {
  throw new Error("Unable to find 'appsettings.dev-env.json'");
}
appsettingsFile = files[0];

const replaceKey = /\!URL_GATEWAY/g;
const replaceValue = process.env.URL_GATEWAY;
const environmentFiles = [environmentTsFile, appsettingsFile];
const environmentFilesFormat = 'utf-8';

for (const environmentFile of environmentFiles) {
  const fileContent = readFileSync(environmentFile, environmentFilesFormat);
  writeFileSync(
    environmentFile,
    fileContent.replace(replaceKey, replaceValue),
    environmentFilesFormat
  );
}

console.info(`Environment Variables were replaced in ${environmentFiles.map(filePath => `"${path.basename(filePath)}"`).join(', ')} files`);
