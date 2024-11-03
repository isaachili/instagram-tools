import colors from 'tailwindcss/colors';

export const content = ['../**/*.{html,razor}'];
export const theme = {
	colors: {
		transparent: 'transparent',
		sky: colors.sky,
		slate: colors.slate,
		red: {
			400: colors.red[400],
		},
		orange: {
			300: colors.orange[300],
		},
		mustard: '#ffc84c',
		midnight: '#0e1825',
	},
	fontFamily: {
		'mono': [ 'JetBrains Mono' ],
		'sans': [ 'Raleway' ]
	},
	extend: {
		gridTemplateColumns: {
			'1fr-3fr': '1fr 3fr',
		},
		keyframes: {
			'progressBar': {
				'0%': { width: '0' },
				'100%': { width: '100%' },
			}
		},
		animation: {
			'progress-bar': 'progressBar 1s ease-in'
		}
	}
};
export const plugins = [];
