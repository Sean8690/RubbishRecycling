import {
  OrganisationClient,
  CountryClient,
  QuoteClient,
  PersonLookupClient
} from './api.generated';

import { httpMiddleware } from './middleware';
const API_BASE_PATH = '/service/cdd/api';
export const OrganisationApi = new OrganisationClient(API_BASE_PATH, httpMiddleware);
export const CountryApi = new CountryClient(API_BASE_PATH, httpMiddleware);
export const QuoteApi = new QuoteClient(API_BASE_PATH, httpMiddleware);
export const PersonSearchApi = new PersonLookupClient(API_BASE_PATH, httpMiddleware);
