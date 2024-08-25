require('dotenv').config();
const fs = require('fs').promises;
const path = require('path');
const csvParser = require('fast-csv');
const { Translate } = require('@google-cloud/translate').v2;

if (!process.env.GOOGLE_API_KEY) {
    console.error(`Google API key not found! Please create a .env file in the root of your project with the following content:
    
GOOGLE_API_KEY=your-google-api-key

You can refer to the .env.example file for guidance.`);
    process.exit(1);
}

const translate = new Translate({ key: process.env.GOOGLE_API_KEY });

const languageCodes = {
    german: 'de',
    spanish: 'es',
    french: 'fr',
    italian: 'it',
    japanese: 'ja',
    koreana: 'ko',
    polish: 'pl',
    brazilian: 'pt',
    russian: 'ru',
    turkish: 'tr',
    schinese: 'zh-CN',
    tchinese: 'zh-TW'
};

async function batchTranslate(texts, targetLang) {
    try {
        const [translations] = await translate.translate(texts, targetLang);
        return Array.isArray(translations) ? translations : [translations];
    } catch (error) {
        throw new Error(`Batch translation failed for language: ${targetLang}. Error: ${error.message}`);
    }
}

async function processCSV(filePath) {
    try {
        const rows = [];
        console.log('Reading CSV file...');
        const csvData = await fs.readFile(filePath, 'utf8');

        await new Promise((resolve, reject) => {
            csvParser.parseString(csvData, { headers: true })
                .on('data', (row) => rows.push(row))
                .on('error', reject)
                .on('end', resolve);
        });

        console.log(`CSV file successfully read. Total rows: ${rows.length}`);

        for (const column in languageCodes) {
            console.log(`Starting batch translation for ${column}...`);

            // Collect texts to be translated, filtering out empty or invalid English texts
            const englishTexts = rows.map(row => row.english && row.english.trim() !== '' ? row.english : null);
            const textsToTranslate = englishTexts.filter(text => text !== null);

            if (textsToTranslate.length === 0) {
                console.log(`No valid English texts to translate for ${column}. Skipping...`);
                continue;
            }

            try {
                const translatedTexts = await batchTranslate(textsToTranslate, languageCodes[column]);

                let translationIndex = 0;
                for (let i = 0; i < rows.length; i++) {
                    if (englishTexts[i] !== null) {
                        rows[i][column] = translatedTexts[translationIndex];
                        console.log(`Translated row ${i + 1} from English: "${englishTexts[i]}" to ${column}: "${translatedTexts[translationIndex]}"`);
                        translationIndex++;
                    }
                }

                console.log(`Batch translation for ${column} completed successfully.`);
            } catch (error) {
                console.error(`Error during batch translation for ${column}:`, error.message);
                process.exit(1);
            }
        }

        console.log('All rows translated. Writing to file...');
        const csvString = await new Promise((resolve, reject) => {
            const output = [];
            const csvStream = csvParser.format({ headers: true });
            csvStream
                .on('data', (chunk) => output.push(chunk))
                .on('error', reject)
                .on('end', () => resolve(output.join('')));

            rows.forEach(row => csvStream.write(row));
            csvStream.end();
        });

        await fs.writeFile(filePath, csvString, 'utf8');
        console.log(`Translation complete. File saved to ${filePath}`);
    } catch (error) {
        console.error('Error during CSV processing:', error.message);
        process.exit(1);
    }
}

const filePath = path.join(__dirname, '..', 'ImmersiveCrosshair', 'Config', 'Localization.txt');
processCSV(filePath);
