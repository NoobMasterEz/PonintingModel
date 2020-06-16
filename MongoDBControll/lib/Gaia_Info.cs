using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.GeoJsonObjectModel;

namespace MongoDBControll.lib
{
   public class GaiaInfo
    {
        /* : this is static value  for we get data from database .
         * 
         * 
         */
        public Object id;
            public long solution_id;
            public string designation;
            public long source_id;
            public int random_index;
            public double ref_epoch;
            public double ra;
            public double ra_error;
            public double dec;
            public double dec_error;
            public double parallax;
            public double parallax_error;
            public double parallax_over_error;
            public double pmra;
            public double pmra_error;
            public double pmdec;
            public double pmdec_error;
            public double ra_dec_corr;
            public double ra_parallax_corr;
            public double ra_pmra_corr;
            public double ra_pmdec_corr;
            public double dec_parallax_corr;
            public double dec_pmra_corr;
            public double dec_pmdec_corr;
            public double parallax_pmra_corr;
            public double parallax_pmdec_corr;
            public double pmra_pmdec_corr;
            public int astrometric_n_obs_al;
            public int astrometric_n_obs_ac;
            public int astrometric_n_good_obs_al;
            public int astrometric_n_bad_obs_al;
            public double astrometric_gof_al;
            public double astrometric_chi2_al;
            public double astrometric_excess_noise;
            public double astrometric_excess_noise_sig;
            public int astrometric_params_solved;
            public string astrometric_primary_flag;
            public double astrometric_weight_al;
            public double astrometric_pseudo_colour;
            public double astrometric_pseudo_colour_error;
            public double mean_varpi_factor_al;
            public int astrometric_matched_observations;
            public int visibility_periods_used;
            public double astrometric_sigma5d_max;
            public int frame_rotator_object_type;
            public int matched_observations;
            public string duplicated_source;
            public int phot_g_n_obs;
            public double phot_g_mean_flux;
            public double phot_g_mean_flux_error;
            public double phot_g_mean_flux_over_error;
            public double phot_g_mean_mag;
            public int phot_bp_n_obs;
            public double phot_bp_mean_flux;
            public double phot_bp_mean_flux_error;
            public double phot_bp_mean_flux_over_error;
            public double phot_bp_mean_mag;
            public int phot_rp_n_obs;
            public double phot_rp_mean_flux;
            public double phot_rp_mean_flux_error;
            public double phot_rp_mean_flux_over_error;
            public double phot_rp_mean_mag;
            public double phot_bp_rp_excess_factor;
            public int phot_proc_mode;
            public double bp_rp;
            public double bp_g;
            public double g_rp;
            public string radial_velocity;
            public string radial_velocity_error;
            public int rv_nb_transits;
            public string rv_template_teff;
            public string rv_template_logg;
            public string rv_template_fe_h;
            public string phot_variable_flag;
            public double l;
            public double b;
            public double ecl_lon;
            public double ecl_lat;
            public string priam_flags;
            public string teff_val;
            public string teff_percentile_lower;
            public string teff_percentile_upper;
            public string a_g_val;
            public string a_g_percentile_lower;
            public string a_g_percentile_upper;
            public string e_bp_min_rp_val;
            public string e_bp_min_rp_percentile_lower;
            public string e_bp_min_rp_percentile_upper;
            public string flame_flags;
            public string radius_val;
            public string radius_percentile_lower;
            public string radius_percentile_upper;
            public string lum_val;
            public string lum_percentile_lower;
            public string lum_percentile_upper;
            public GeoJsonPoint<GeoJson2DGeographicCoordinates> location;
          
    }
}
