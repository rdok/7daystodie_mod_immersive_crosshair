const { readFileSync, unlinkSync, rmSync, cpSync } = require("fs");
const { join } = require("path");
const { execSync } = require("child_process");
require("dotenv").config();

const packageJsonPath = join(__dirname, "..", "package.json");
const packageJsonRaw = readFileSync(packageJsonPath, "utf8");
const packageJson = JSON.parse(packageJsonRaw);
const version = packageJson.version;
const artifact = `${process.env.NO_CROSSHAIR} ${version}.7z`;
const srcDir = join(__dirname, "../NoCrosshair/bin/Release");
const distDir = join(__dirname, "..", "dist");
const buildDir = join(distDir, process.env.NO_CROSSHAIR);

try {
  unlinkSync(artifact);
  rmSync(distDir, { recursive: true });
} catch (e) {
  // Expected behaviour if it doesn't exist.
}

cpSync(srcDir, buildDir, { recursive: true });

execSync(
  `.\\node_modules\\7z-bin\\win32\\7z.exe a "${artifact}" "${buildDir}"`,
);
