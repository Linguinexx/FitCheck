const { chromium } = require('playwright');
const { pathToFileURL } = require('node:url');
const { resolve } = require('node:path');
(async () => {
  const browser = await chromium.launch({executablePath: 'C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe'});
  try {
    const page = await browser.newPage({viewport:{width:1100,height:900},deviceScaleFactor:1});
    const url = pathToFileURL(resolve(process.argv[2], 'logo_motion.html')).href;
    const frames = {};
    for (const t of [0,320,520,760,1120,1600,'static']) {
      await page.goto(url + (t==='static' ? '?static=1' : '?t='+t));
      await page.waitForFunction(() => window.__p2mReady);
      frames[t]=await page.locator('#logo-root').screenshot({path:resolve(process.argv[2],'outputs',t+'.png')});
    }
    if (!frames[1600].equals(frames.static)) throw new Error('Final frame differs from static');
    console.log('Final frame equals static');
  } finally { await browser.close(); }
})().catch(error => { console.error(error); process.exitCode=1; });
