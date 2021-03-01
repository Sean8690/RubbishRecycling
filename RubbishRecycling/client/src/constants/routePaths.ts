export const BASE_PATH = '/service/cdd';
export const LEAP_BASE_PATH = '/newwebsite/service/cdd';

/** As all route parameters are going to be mandatory, lets compare against this value to find out if some are null/undefined */
export const EMPTY_PARAM_VALUE = '-1';

export enum ROUTES {
  CDD_SEARCH = '/',
  CDD_SEARCH_BACK = '/search/:matterReference/:countryCode/:companyName/:companyNumber',
  /**All parameters are mandatory, for null or undefined, set parameters to EMPTY_PARAM_VALUE */
  CDD_LOOKUP_RESULT = '/lookup/:matterReference/:countryCode/:companyName/:companyNumber',
  AML_SEARCH = '/aml',
  AML_LOOKUP_RESULT = '/aml/results'
}
