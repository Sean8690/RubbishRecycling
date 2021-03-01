/** @jsx jsx */
import { Checkbox, Table, TableDataRow } from '@infotrack/zenith-ui';
import { Dispatch, SetStateAction } from 'react';
import { jsx } from 'theme-ui';
import { IPersonLite } from '../../api/api.generated';
import { toAusDateFormat } from '../../utils';
import * as css from './lookupResultCss';

interface PersonTableRow extends IPersonLite, TableDataRow {}

interface CompaniesTableProps {
  setSelectedCompanies: Dispatch<SetStateAction<Set<string>>>;
  selectedCompanies: Set<string>;
  tableRows: TableDataRow[];
}

export function PersonsTable(props: CompaniesTableProps) {
  const {
    setSelectedCompanies: setSelectedCompanies,
    tableRows,
    selectedCompanies
  } = props;

  const handleRowClick = (row: PersonTableRow, checked?: boolean) => {
    if (!row._selected || checked) {
      setSelectedCompanies(
        new Set(selectedCompanies.add(row.providerEntityCode as string))
      );
    }
    if (row._selected || !checked) {
      selectedCompanies.delete(row.providerEntityCode as string);
      setSelectedCompanies(new Set(selectedCompanies));
    }
  };
  return (
    <div sx={css.tableWrapper}>
      <Table
        rows={tableRows}
        rowKeyPropName="providerEntityCode"
        rowOnClick={handleRowClick as (row: PersonTableRow) => void}
        columns={[
          {
            heading: 'Name',
            propName: 'fullName',
            columnWidth: '30rem',
            transform: (c: PersonTableRow) => (
              <div>
                <Checkbox
                  checked={c._selected}
                  onChange={(e) => {
                    handleRowClick(c, e.target.checked);
                  }}>
                  {c.fullName ?? '-'}
                </Checkbox>
              </div>
            )
          },
          {
            heading: 'Date/Year Of Birth',
            propName: 'dob',
            transform: (c: PersonTableRow) => (
              <span>
                {c.dob && c.dob.length > 4
                  ? toAusDateFormat(c.dob)
                  : c.dob || c.yearOfBirth || '-'}
              </span>
            )
          },
          {
            heading: 'Country',
            propName: 'country',
            transform: (c: PersonTableRow) => (
              <span>{c?.countries?.join(', ') ?? '-'}</span>
            )
          }
        ]}
      />
    </div>
  );
}
