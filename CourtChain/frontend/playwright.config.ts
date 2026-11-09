import { defineConfig } from '@playwright/test'
import { randomUUID } from 'node:crypto'
import { tmpdir } from 'node:os'
import { join } from 'node:path'
const password = process.env.E2E_PASSWORD || 'Test-' + randomUUID()
process.env.E2E_PASSWORD = password
export default defineConfig({
  testDir: './e2e',
  timeout: 90000,
  workers: 1,
  reporter: 'list',
  use: {
    baseURL: 'http://127.0.0.1:5175',
    viewport: { width: 1440, height: 1000 },
    launchOptions: { executablePath: process.env.BROWSER_PATH || 'C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe' },
    screenshot: 'only-on-failure',
  },
  webServer: [
    { command: 'dotnet run --no-build --project ../backend --no-launch-profile --urls http://127.0.0.1:5100',
      url: 'http://127.0.0.1:5100/openapi/v1.json', timeout: 30000,
      env: { COURTCHAIN_OWNER_PASSWORD: password, ConnectionStrings__CourtChain: 'Data Source=' + join(tmpdir(), 'courtchain-e2e-' + randomUUID() + '.db') } },
    { command: 'npm run dev -- --port 5175', url: 'http://127.0.0.1:5175', timeout: 30000,
      env: { API_PROXY_TARGET: 'http://127.0.0.1:5100' } },
  ],
})
