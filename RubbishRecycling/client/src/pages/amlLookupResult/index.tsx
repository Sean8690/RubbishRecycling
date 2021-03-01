/** @jsx jsx */
import {
  Breadcrumb,
  Button,
  Icon,
  Message,
  TableDataRow,
  Text,
  useSpinner
} from '@infotrack/zenith-ui';
import { Fragment, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { Flex, jsx } from 'theme-ui';
import { AmlPersonLookupReportRequest, PersonSearchApi } from '../../api';
import { ContentWrapper, Divider, Header } from '../../components';
import { ROUTES } from '../../constants';
import { useDowJonesContext } from '../../contexts';
import * as css from './lookupResultCss';
import { PersonsTable } from './personsTable';
import { getUrlPath } from '../../utils';

export function DowJonesLookupResult() {
  const [selectedCompanies, setSelectedCompanies] = useState<Set<string>>(new Set());
  const { lookupResults, searchedName } = useDowJonesContext()[0];
  const loader = useSpinner({ fullScreen: true });
  const routerHistory = useHistory();
  const handleOrder = async () => {
    loader.show();
    const request = new AmlPersonLookupReportRequest({
      orderId: lookupResults.orderId as number,
      providerEntityCodes: Array.from(selectedCompanies)
    });
    await PersonSearchApi.generateReport(request);
    window.location.href =
      getUrlPath() + `/Order/Show/${lookupResults.orderId as number}`;
    loader.hide();
  };

  const tableRows: TableDataRow[] =
    lookupResults.matches?.map((person) => {
      return {
        ...person,
        _selected: selectedCompanies.has(person.providerEntityCode as string)
      };
    }) ?? [];

  // if got here by accident or refreshed page
  if (!lookupResults.clientReference) {
    return (
      <div sx={{ pt: ['4rem', '8rem'] }}>
        <Message
          buttonProps={{
            children: 'Go to search form',
            onClick: () => routerHistory.push(ROUTES.AML_SEARCH)
          }}
          illustration="empty"
          size="small"
          subtitle="Please try searching again"
          title="Nothing to show"
        />
      </div>
    );
  }

  return (
    <Fragment>
      <Header>
        <Breadcrumb
          subtitle={lookupResults.clientReference}
          pathRoot="Peps & Sanctions"
          paths={[searchedName]}
        />
      </Header>
      <ContentWrapper sx={css.mainWrapperCss}>
        {!lookupResults.matches?.length && (
          <div sx={{ pt: ['4rem', '8rem'] }}>
            <Message
              buttonProps={{
                children: 'Go to search form',
                onClick: () => routerHistory.push(ROUTES.AML_SEARCH)
              }}
              illustration="empty"
              size="small"
              subtitle="Please try searching again"
              title="Nothing to show"
            />
          </div>
        )}
        {!!lookupResults.matches?.length && (
          <Fragment>
            <div sx={css.marginBottomCss}>
              <Text as="h1" variant="feature">
                <span sx={css.iconWrapperCss}>
                  <Icon name="business" />
                </span>
                {!!lookupResults.matches?.length && (
                  <span>
                    Showing {lookupResults.matches.length}
                    {lookupResults.matches.length > 1 ? ' results' : ' result'}
                  </span>
                )}
              </Text>
            </div>
            <PersonsTable
              tableRows={tableRows}
              setSelectedCompanies={setSelectedCompanies}
              selectedCompanies={selectedCompanies}
            />

            <Divider sx={{ mt: '2rem' }} />
            <Flex sx={{ justifyContent: 'flex-end' }}>
              <Button
                size="large"
                variant="primary"
                type="button"
                disabled={selectedCompanies.size < 1}
                onClick={handleOrder}>
                Prepare report
              </Button>
            </Flex>
          </Fragment>
        )}
      </ContentWrapper>
      {loader.spinner}
    </Fragment>
  );
}
