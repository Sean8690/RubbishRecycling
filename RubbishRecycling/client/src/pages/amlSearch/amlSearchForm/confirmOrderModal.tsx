/** @jsx jsx */
import { Modal } from '@infotrack/zenith-ui';
import { FormikProps } from 'formik';
import { jsx } from 'theme-ui';
import { IAmlPersonLookupRequest } from '../../../api';
import { InformationDetailRow } from '../../../components';
import { toAusDateFormat } from '../../../utils';

interface ConfirmOrderModalProps {
  formik: FormikProps<IAmlPersonLookupRequest>;
  closeModalFunction: () => void;
}
export function ConfirmOrderModal(props: ConfirmOrderModalProps) {
  const { formik, closeModalFunction } = props;
  const requestInfo = formik.values;
  const handleProceed = () => {
    void formik.submitForm();
    closeModalFunction();
  };
  return (
    <Modal
      title="Confirm order details"
      isOpen
      subtitle="Ensure the below details are correct"
      primaryButtonProps={{
        children: 'Proceed',
        onClick: handleProceed
      }}
      secondaryButtonProps={{
        children: 'Cancel',
        onClick: closeModalFunction
      }}
      icon="description">
      <div sx={{ padding: '1.5rem 0' }}>
        <InformationDetailRow label="Matter" value={requestInfo.clientReference} />
        <InformationDetailRow label="Given Name" value={requestInfo.givenName} />
        <InformationDetailRow label="Middle Name" value={requestInfo.middleName || '-'} />
        <InformationDetailRow label="Family Name" value={requestInfo.familyName} />
        {requestInfo.dateOfBirth && (
          <InformationDetailRow
            label="Date of Birth"
            value={toAusDateFormat(requestInfo.dateOfBirth)}
          />
        )}
        {requestInfo.yearOfBirth && (
          <InformationDetailRow label="Year of Birth" value={requestInfo.yearOfBirth} />
        )}
      </div>
    </Modal>
  );
}
