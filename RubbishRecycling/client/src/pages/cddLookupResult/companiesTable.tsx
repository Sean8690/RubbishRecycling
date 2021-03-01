/** @jsx jsx */
import { RadioButton, Table, TableDataRow } from '@infotrack/zenith-ui';
import { Dispatch, SetStateAction } from 'react';
import { jsx } from 'theme-ui';
import { OrganisationLiteDto } from '../../api/api.generated';
import * as css from './lookupResultCss';

interface CopmanyTableDataRow extends OrganisationLiteDto, TableDataRow {}

interface CompaniesTableProps {
  setSelectedCompany: Dispatch<SetStateAction<CopmanyTableDataRow | null>>;
  tableRows: TableDataRow[];
}

export function CompaniesTable(props: CompaniesTableProps) {
  const { setSelectedCompany, tableRows } = props;
  return (
    <div sx={css.tableWrapper}>
      <Table
        rows={tableRows}
        rowKeyPropName="providerEntityCode"
        rowOnClick={(row) => setSelectedCompany(row as CopmanyTableDataRow)}
        columns={[
          {
            heading: 'Company Name',
            propName: 'name',
            columnWidth: '25rem',
            transform: (c: CopmanyTableDataRow) => (
              <div>
                <RadioButton
                  checked={c._selected}
                  onChange={(e) => {
                    console.log('executingOnchangeSimple');
                    if (e.target.checked) setSelectedCompany(c);
                  }}>
                  {c.name ?? 'Not provided'}
                </RadioButton>
              </div>
            )
          },
          {
            heading: 'Company number',
            propName: 'number',
            transform: (c: OrganisationLiteDto) => (
              <span>{c.number ?? 'Not provided'}</span>
            )
          },
          {
            heading: 'Address',
            propName: 'address',
            transform: (c: OrganisationLiteDto) => (
              <span>{c.address ? c.address : 'Not provided'}</span>
            )
          }
        ]}
      />
    </div>
  );
}
