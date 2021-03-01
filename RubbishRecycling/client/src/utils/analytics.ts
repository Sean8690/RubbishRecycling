import { Analytics } from '@infotrack/analytics';

const analyticsApplication = 'infotrack-cdd';

export const InfotrackAnalytics = new Analytics({
  application: analyticsApplication,
  trackingId: 'UA-174145598-2'
});

export const analyticsLabels = {
  ubo: 'ubo'
};
