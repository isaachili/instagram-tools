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
};
export const plugins = [];
