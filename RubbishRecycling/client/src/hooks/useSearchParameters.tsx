import { useParams } from 'react-router-dom';
import { EMPTY_PARAM_VALUE } from '../constants';
import { SearchInputs } from '../models';
import { useMappingContext } from '@infotrack/zenith-ui';

export function useSearchParameters(): SearchInputs {
  const mappingContext = useMappingContext();
  const clientReference = mappingContext.findValueForMappingPath('ClientReference');

  let { companyName, companyNumber } = useParams<SearchInputs>();
  const { matterReference, countryCode } = useParams<SearchInputs>();
  companyNumber = companyNumber === EMPTY_PARAM_VALUE ? '' : companyNumber;
  companyName = companyName === EMPTY_PARAM_VALUE ? '' : companyName;

  return {
    matterReference: clientReference
      ? clientReference
      : matterReference
      ? decodeURIComponent(matterReference)
      : '',
    countryCode: countryCode ? decodeURIComponent(countryCode).toUpperCase() : '',
    companyName: companyName ? decodeURIComponent(companyName) : '',
    companyNumber: companyNumber ? decodeURIComponent(companyNumber) : ''
  };
}
