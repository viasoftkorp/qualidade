import { defineConfig } from 'cypress'
import cypressConfig from './cypress-config.json'

const setupNodeEvents = (on, config) => {
  return require('./cypress/plugins/index.js')(on, config)
}

export default defineConfig({
  ...cypressConfig,
  e2e: {
    setupNodeEvents,
    ...cypressConfig.e2e
  }
})
