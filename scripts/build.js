const {unlinkSync, rmSync, cpSync} = require("fs");
const {join} = require("path");
const {execSync} = require("child_process");

const args = process.argv.slice(2);
if (args.length < 2) {
    console.error("Usage: node script.js <absoluteOutputPath> <modName> <modVersion>");
    process.exit(1);
}

console.debug(args);
const absoluteOutputPath = args[0];
const newModName = args[1];
const newVersion = args[2];

const artifact = `${newModName}_${newVersion}.7z`;
const distDir = join(__dirname, "..", "dist");
const buildDir = join(distDir, newModName);

try {
    unlinkSync(artifact);
    rmSync(distDir, {recursive: true});
} catch (e) {
    // Expected behavior if it doesn't exist.
}

cpSync(absoluteOutputPath, buildDir, {recursive: true});

execSync(
    `.\\node_modules\\7z-bin\\win32\\7z.exe a "${artifact}" "${buildDir}"`
);

console.log(`Build completed: ${artifact}`);
