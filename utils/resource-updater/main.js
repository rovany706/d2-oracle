const vdf = require('@node-steam/vdf')
const fs = require('node:fs');

let vdf_data = fs.readFileSync('temp/items.txt', 'utf8');
vdf_data = vdf_data.replaceAll('"ItemRequirements"\r\n\t\t""', '"ItemRequirements" ""');

const items = vdf.parse(vdf_data)['DOTAAbilities'];
let o = []

for (const [key, value] of Object.entries(items)) {
    if (key === 'Version') continue;
    if (value['ItemCost'] == undefined) continue;
    o.push({
        'name': key,
        'cost': value['ItemCost'].toString()
    })
}

fs.writeFileSync('../../src/D2Oracle/D2Oracle.Core/Resources/items.json', JSON.stringify(o, null, 4), 'utf-8');