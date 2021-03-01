import moment from 'moment';
import 'moment/locale/en-au';

moment.locale('en-au');

export const longDateFormat = 'DD/MM/YYYY';
export const serverDateFormat = 'YYYY-MM-DD';
export const queryStringDateFormat = 'YYYY.MM.DD';
export const longTimeFormat = 'hh:mm:ssa';
export const shortTimeFormat = 'hh:mma';

/** e.g. 19/09/2019 12:10:15pm */
export const toLongDateTimeString = (inp?: moment.MomentInput): string | undefined =>
  inp ? moment(inp).format(`${longDateFormat} ${longTimeFormat}`) : undefined;

/** e.g 23/03/1990 */
export const toAusDateFormat = (inp?: moment.MomentInput): string | undefined =>
  inp ? moment(inp).format(longDateFormat) : undefined;

/** YYYY.MM.DD Use periods instead of slashes to avoid cryptic urlencoded values */
export const toQueryDateString = (inp?: moment.MomentInput): string | undefined =>
  inp ? moment(inp).format(queryStringDateFormat) : undefined;

/** @param date queryStringDateFormat  */
export const queryStringToDate = (date: string): Date | undefined => {
  if (['null', 'undefined', ''].includes(String(date).trim())) return undefined;
  return moment(date, queryStringDateFormat).toDate();
};

/** To avoid bugs caused by invalid date objects -> https://stackoverflow.com/questions/1353684/detecting-an-invalid-date-date-instance-in-javascript */
export const isValidDateInstance = (date?: Date) => {
  return date instanceof Date && !isNaN(date.getTime());
};

/** Removes the time zone 🕕 for a date so it can get to the server with the value that the user sees in the screen */
export const removeTimeZone = (date?: Date) => {
  if (!date) return moment.invalid();
  const momentDate = moment(date);
  return moment(momentDate).utc().add(momentDate.utcOffset(), 'm').toDate();
};

/** Gets the string value representing the text the use has entered without the mask characters */
const getCleanStringFromMaskedDate = (value: string) => {
  if (!value || typeof value !== 'string') return '';
  return value.replace(/\s|-/g, '').replace(/^\/|\/$|\/\//g, '');
};

/** Returns a date based on a string, if the string is not a valid date, it will return null */
const FORMAT = 'DD/MM/YYYY';
export const getDateFromString = (str?: string, format = FORMAT) => {
  if (!str || getCleanStringFromMaskedDate(str).length < FORMAT.length) return null;
  const date = removeTimeZone(moment(str, format).toDate()) as Date;
  if (isValidDateInstance(date)) return date;
  return null;
};

export const getTimeDifferenceFromNow = (date?: Date) => {
  return moment(date).fromNow();
};

export const hasDateExpired = (date?: Date) => {
  return moment().isAfter(date);
};

export const getDisplayDate = (str: string) => {
  return moment(str)?.format(longDateFormat);
};

export const getDisplayTime = (str: string) => {
  return moment(str)?.format(shortTimeFormat);
};

export const timestampToLocaleAwareIsoDateString = (timestamp: number) => {
  return moment(timestamp).format();
};

export const getCurrentLocaleAwareIsoDateString = () => {
  return moment().format();
};

export const subtractYearsFromNow = (years: number) => {
  return moment().subtract(years, 'years').toDate();
};

export function isDateInThePast(date: Date) {
  return moment(date).isSameOrBefore(moment(), 'day');
}

/**Gets a date object formatted as YYYY-MM-DD to be sent to the server */
export function getDateStringForRequest(date?: Date) {
  if (!date || !isValidDateInstance(date)) return '';
  return moment(date).format(serverDateFormat);
}

export const momentUtil = moment;
