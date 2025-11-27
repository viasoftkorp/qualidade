// Visual Regression Test command
import { addHelperCommands } from '@viasoft/testing';
import { addMatchImageSnapshotCommand } from 'cypress-image-snapshot/command';

addMatchImageSnapshotCommand();
addHelperCommands();

Cypress.Commands.add('batchRequestStub', (requestsToStub) => {
  const aliases = [];
  if (!requestsToStub) {
    return cy.wrap(aliases);
  }
  let normalizedRequestsToStub = requestsToStub;
  if (!Array.isArray(requestsToStub)) {
    normalizedRequestsToStub = [requestsToStub]
  }

  for (let i = 0; i < normalizedRequestsToStub.length; i++) {
    const request = normalizedRequestsToStub[i];
    const requestAlias = `${request.method}-${request.url}-${Math.round(Math.random() * 1000)}`;
    cy.intercept(request.method, request.url, request.response).as(requestAlias);
    aliases.push(`@${requestAlias}`);
  }
  return cy.wrap(aliases);
});
Cypress.Commands.add('validateRequestBody', (body, response) => {
  expect(body).to.deep.include(response);
});
Cypress.Commands.add('validateRequestStatusCode', (statusCode, expectedStatusCode) => {
  expect(statusCode).to.eql(expectedStatusCode)
});
Cypress.Commands.add('getVsCheckbox', (formControlName, parentSelector = '') => {
  cy.get(`${parentSelector} vs-checkbox[formControlName=${formControlName}] mat-checkbox input`)
})
Cypress.Commands.add('getAutoCompleteItem', (options) => {
  cy.batchRequestStub(options.request).then(alias => {
    cy.get(`${options.selector}[formControlName=${options.formControlName}] input`).click()
    cy.wait(alias)
    cy.get('div.vs-autocomplete-panel vs-button button').eq(options.eq | 0).click()

  })
})
Cypress.Commands.add('getVsTextArea', (formControlName, parentSelector = '') => {
  cy.get(`${parentSelector} vs-textarea[formControlName=${formControlName}] textarea`)
})
Cypress.Commands.add('getVsTabGroupItem', (itemLabel, parentSelector = '') => {
  cy.get(`${parentSelector} mat-tab-group mat-tab-header div`).contains(itemLabel)
})
Cypress.Commands.add('getVsInput', (formControlName, parentSelector = '') => {
  cy.get(`${parentSelector} vs-input[formControlName=${formControlName}] input`)
})
Cypress.Commands.add('getVsDatePicker', (formControlName, parentSelector = '') => {
  cy.get(`${parentSelector} vs-datepicker[ng-reflect-control-name=${formControlName}]`)
})

