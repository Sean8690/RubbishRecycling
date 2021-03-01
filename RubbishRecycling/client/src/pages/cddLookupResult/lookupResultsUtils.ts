import { useEffect, useState } from 'react';
import { OrganisationLiteDto } from '../../api/api.generated';
import { OrganisationApi } from '../../api/clientApi';

export function useLookupApiCall(
  companyName: string,
  companyNumber: string,
  countryCode: string
) {
  const [results, setResults] = useState<OrganisationLiteDto[]>([]);
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    async function lookup() {
      const apiResponse = await OrganisationApi.get(
        companyName,
        companyNumber,
        countryCode
      );
      setResults(apiResponse);
      setLoading(false);
    }
    void lookup();
  }, [companyName, companyNumber, countryCode]);

  return { results, loading };
}
