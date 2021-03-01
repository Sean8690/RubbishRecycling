import { useEffect, useState } from 'react';
import { QuoteDto, ServiceIdentifier2 } from '../api/api.generated';
import { QuoteApi } from '../api/clientApi';

export function useGetQuote(
  countryCode: string | null,
  serviceIdentifier: ServiceIdentifier2
) {
  const [quote, setQuote] = useState<QuoteDto | null>(null);

  useEffect(() => {
    const getFee = async () => {
      const feeResponse = await QuoteApi.fee(serviceIdentifier, countryCode);
      setQuote(feeResponse);
    };
    void getFee();
  }, [countryCode, serviceIdentifier]);

  return { quote };
}
