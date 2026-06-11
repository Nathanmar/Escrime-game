// commitlint.config.js
const REQ_PATTERN = /^REQ-[A-Z]-\d{3}$/; // ex: REQ-A-001
const TC_PATTERN  = /^TC-\d{3}$/;        // ex: TC-001
const TC_TOKEN    = /TC-[A-Za-z0-9]+/g;
const TDD_TYPES   = ['red', 'green'];

module.exports = {
  extends: ['@commitlint/config-conventional'],
  plugins: [
    {
      rules: {
        'tdd/req-scope': ({ type, scope }) => {
          if (!TDD_TYPES.includes(type)) return [true];
          return [
            Boolean(scope) && REQ_PATTERN.test(scope),
            `un commit "${type}" doit cibler un REQ valide, ex: ${type}(REQ-A-001): ...`,
          ];
        },
        'tdd/red-requires-tc': ({ type, header, body }) => {
          if (type !== 'red') return [true];
          const text = `${header || ''}\n${body || ''}`;
          return [
            (text.match(TC_TOKEN) || []).some((t) => TC_PATTERN.test(t)),
            'un commit "red" doit référencer au moins un cas de test, ex: TC-001',
          ];
        },
        'tdd/tc-format': ({ header, body }) => {
          const text = `${header || ''}\n${body || ''}`;
          const bad = (text.match(TC_TOKEN) || []).filter((t) => !TC_PATTERN.test(t));
          return [
            bad.length === 0,
            `cas de test mal formé(s): ${bad.join(', ')} (attendu: TC-001)`,
          ];
        },
      },
    },
  ],
  rules: {
    'type-enum': [2, 'always', ['red', 'green', 'refactor', 'chore', 'docs', 'ci', 'test', 'feat', 'fix']],
    'type-empty': [2, 'never'],
    'subject-empty': [2, 'never'],
    'scope-case': [0],
    'subject-case': [0],
    'tdd/req-scope': [2, 'always'],
    'tdd/red-requires-tc': [2, 'always'],
    'tdd/tc-format': [2, 'always'],
  },
};

