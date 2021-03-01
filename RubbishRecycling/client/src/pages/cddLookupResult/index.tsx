/** @jsx jsx */
import {
  Breadcrumb,
  Icon,
  Spinner,
  TableDataRow,
  Text,
  useMappingContext
} from '@infotrack/zenith-ui';
import { Fragment, useState } from 'react';
import { generatePath, useHistory } from 'react-router-dom';
import { Flex, jsx } from 'theme-ui';
import {
  OrganisationLiteDto,
  ServiceIdentifier,
  ServiceIdentifier2
} from '../../api/api.generated';
import { OrganisationApi } from '../../api/clientApi';
import { ContentWrapper, Header } from '../../components';
import { EMPTY_PARAM_VALUE, ROUTES } from '../../constants';
import { useGetQuote, useSearchParameters } from '../../hooks';
import { ActionsSection } from './actionsSection';
import { CompaniesTable } from './companiesTable';
import * as css from './lookupResultCss';
import { useLookupApiCall } from './lookupResultsUtils';
import { NoResults } from './noResults';
import { getUrlPath } from '../../utils';

export type CddLookupRouteParams = {
  country: string;
  companyName: string;
  companyNumber: string;
  matterReference: string;
};

export function CddLookupResult() {
  const {
    matterReference,
    companyName,
    companyNumber,
    countryCode
  } = useSearchParameters();
  const { results, loading } = useLookupApiCall(
    companyName as string,
    companyNumber as string,
    countryCode
  );
  const routeHistory = useHistory();
  const [selectedCompany, setSelectedCompany] = useState<OrganisationLiteDto | null>(
    null
  );

  /** ie. Infotrack / 148789 , or just Infotrack */
  const breadcrumbPath = `${companyName ? companyName : ''} ${
    companyNumber ? `/ ${companyNumber}` : ''
  }`;
  const { quote } = useGetQuote(countryCode, ServiceIdentifier2.CddOrganisationReport);
  const [orderLoading, setOrderLoading] = useState(false);
  const onBack = () => {
    const backPath = generatePath(ROUTES.CDD_SEARCH_BACK, {
      companyName: companyName ? companyName : EMPTY_PARAM_VALUE,
      companyNumber: companyNumber ? companyNumber : EMPTY_PARAM_VALUE,
      countryCode,
      matterReference
    });
    routeHistory.push(backPath);
  };

  const tableRows: TableDataRow[] =
    results?.map((company) => ({
      ...company,
      _selected: selectedCompany?.providerEntityCode === company.providerEntityCode
    })) ?? [];

  const mappingContext = useMappingContext();

  const handleOrder = async () => {
    setOrderLoading(true);
    const retailerReference = mappingContext.findValueForMappingPath('RetailerReference');

    const order = await OrganisationApi.post(
      selectedCompany?.providerEntityCode ?? null,
      countryCode,
      (ServiceIdentifier.CddOrganisationReport as unknown) as ServiceIdentifier2,
      matterReference,
      retailerReference,
      quote?.quoteId
    );
    setOrderLoading(false);
    if (order?.orderId)
      window.location.href = getUrlPath() + `/Order/Show/${order.orderId}`;
  };

  return (
    <Fragment>
      <Header>
        <Breadcrumb
          subtitle={matterReference}
          pathRoot="International Company Report"
          paths={[breadcrumbPath]}
        />
      </Header>
      {!loading && results?.length > 0 && (
        <ContentWrapper sx={css.mainWrapperCss}>
          <div sx={css.marginBottomCss}>
            <Text as="h1" variant="feature">
              <span sx={css.iconWrapperCss}>
                <Icon name="business" />
              </span>
              Showing {results.length} result{results.length > 1 ? 's' : ''}
            </Text>
          </div>

          <CompaniesTable tableRows={tableRows} setSelectedCompany={setSelectedCompany} />

          <ActionsSection
            fee={quote?.fee?.toFixed(2) ?? 0}
            hasSelectedCompany={!!selectedCompany}
            onBackClicked={onBack}
            submitHandler={handleOrder}
            isOrdering={orderLoading}
          />
        </ContentWrapper>
      )}

      <Flex sx={css.noResultsWrapperCss}>
        {loading && <Spinner size="large" type="dots" />}
        {!loading && !results?.length && <NoResults onBack={onBack} />}
      </Flex>
    </Fragment>
  );
}
