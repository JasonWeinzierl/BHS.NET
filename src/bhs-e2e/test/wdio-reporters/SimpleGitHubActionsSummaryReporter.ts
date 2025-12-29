import * as core from '@actions/core';
import { SUMMARY_ENV_VAR } from '@actions/core/lib/summary';
import WDIOReporter, { SuiteStats, TestStats } from '@wdio/reporter';
import { Capabilities } from '@wdio/types';
import prettyMilliseconds from 'pretty-ms';

export class SimpleGitHubActionsSummaryReporter extends WDIOReporter {
  private readonly pendingWrites = new Set<string>();

  override get isSynchronised(): boolean {
    return this.pendingWrites.size === 0;
  }

  override onSuiteEnd(suiteStats: SuiteStats): void {
    // eslint-disable-next-line n/no-process-env
    if (!process.env[SUMMARY_ENV_VAR]) return;
    if (suiteStats.parent) return;

    this.pendingWrites.add(suiteStats.uid);

    writeSummary(suiteStats, this.runnerStat?.capabilities)
      .finally(() => {
        this.pendingWrites.delete(suiteStats.uid);
      })
      .catch((e: unknown) => {
        core.error(e as Error);
        core.error(`Failed to report test summary of suite ${suiteStats.uid} to GitHub Actions.`);
      });
  }
}

async function writeSummary(
  suiteStats: SuiteStats,
  capabilities: Capabilities.ResolvedTestrunnerCapabilities | undefined,
): Promise<void> {
  const suiteCapLabel = getSuiteCapabilityLabel(capabilities);

  const allTestStats: TestStats[] = [];
  function collectStats(suite: SuiteStats, prefix?: string) {
    for (const test of suite.tests) {
      test.title = prefix ? `${prefix} > ${test.title}` : test.title;
      allTestStats.push(test);
    }

    for (const childSuite of suite.suites) {
      // Recurse.
      collectStats(childSuite, prefix ? `${prefix} > ${childSuite.title}` : childSuite.title);
    }
  }
  collectStats(suiteStats);

  await core.summary
    .addHeading(`${suiteCapLabel}: ${suiteStats.title}`, 2)
    .addList([
      suiteStats.description ? `<b>Description:</b> ${suiteStats.description}` : null,
      `<b>Duration:</b> ${prettyMilliseconds(suiteStats.duration)}`,
      suiteStats.cid ? `<b>Log Prefix (Capability ID):</b> ${suiteStats.cid}` : null,
      capabilities?.browserName ? `<b>Browser:</b> ${capabilities.browserName as string}` : null,
      capabilities?.browserVersion ? `<b>Browser Version:</b> ${capabilities.browserVersion as string}` : null,
      capabilities?.platformName ? `<b>Platform:</b> ${capabilities.platformName as string}` : null,
      capabilities?.['appium:platformVersion'] ? `<b>Platform Version:</b> ${capabilities['appium:platformVersion'] as string}` : null,
    ].filter(x => x !== null))
    .addTable([
      [
        { data: 'Test', header: true },
        { data: 'Duration', header: true },
        { data: 'Status', header: true },
        { data: 'Message', header: true },
      ],
      ...allTestStats.map(stats => [
        stats.title,
        prettyMilliseconds(stats.duration),
        stats.state,
        stats.error?.message ?? stats.pendingReason ?? '',
      ]),
    ])
    .write();
}

function getSuiteCapabilityLabel(
  capabilities: Capabilities.ResolvedTestrunnerCapabilities | undefined,
): string {
  if (!capabilities) {
    return '<null>';
  }

  const capBrowserName = capabilities.browserName;
  const capPlatformName = capabilities.platformName;

  if (typeof capBrowserName !== 'string' || typeof capPlatformName !== 'string') {
    // multi-remote.
    return '<multiple>';
  } else if (capBrowserName) {
    // browser.
    return capBrowserName;
  } else if (capPlatformName) {
    // mobile.
    return capPlatformName;
  } else {
    return '<unknown>';
  }
}
