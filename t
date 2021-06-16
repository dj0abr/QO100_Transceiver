Library version: 0.21 (git tag: 0cbeeca)
Compiled with backends: local xml ip usb
IIO context created with network backend.
Backend version: 0.21 (git tag: v0.21  )
Backend description string: 192.168.20.25 Linux (none) 4.19.0-g6edc6cd24b48-dirty #15 SMP PREEMPT Sat Jan 23 11:21:25 CET 2021 armv7l
IIO context has 9 attributes:
	hw_model: Analog Devices PlutoSDR Rev.B (Z7010-AD9364)
	hw_model_variant: 0
	hw_serial: 1044730a199700191f001800f3647c5341
	fw_version: v0.32-dirty
	ad9361-phy,xo_correction: 40000000
	ad9361-phy,model: ad9364
	local,kernel: 4.19.0-g6edc6cd24b48-dirty
	uri: ip:192.168.20.25
	ip,ip-addr: 192.168.20.25
IIO context has 5 devices:
	iio:device0: adm1177
		2 channels found:
			voltage0:  (input)
			2 channel-specific attributes found:
				attr  0: raw value: 776
				attr  1: scale value: 6.433105468
			current0:  (input)
			2 channel-specific attributes found:
				attr  0: raw value: 858
				attr  1: scale value: 0.516601562
		No trigger on this device
	iio:device1: ad9361-phy
		9 channels found:
			altvoltage1: TX_LO (output)
			8 channel-specific attributes found:
				attr  0: external value: 0
				attr  1: fastlock_load value: 0
				attr  2: fastlock_recall ERROR: Invalid argument (22)
				attr  3: fastlock_save value: 0 30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30
				attr  4: fastlock_store value: 0
				attr  5: frequency value: 1290250000
				attr  6: frequency_available value: [46875001 1 6000000000]
				attr  7: powerdown value: 0
			voltage0:  (input)
			15 channel-specific attributes found:
				attr  0: bb_dc_offset_tracking_en value: 1
				attr  1: filter_fir_en value: 1
				attr  2: gain_control_mode value: slow_attack
				attr  3: gain_control_mode_available value: manual fast_attack slow_attack hybrid
				attr  4: hardwaregain value: 66.000000 dB
				attr  5: hardwaregain_available value: [-1 1 73]
				attr  6: quadrature_tracking_en value: 1
				attr  7: rf_bandwidth value: 1000000
				attr  8: rf_bandwidth_available value: [200000 1 56000000]
				attr  9: rf_dc_offset_tracking_en value: 1
				attr 10: rf_port_select value: A_BALANCED
				attr 11: rf_port_select_available value: A_BALANCED B_BALANCED C_BALANCED A_N A_P B_N B_P C_N C_P TX_MONITOR1 TX_MONITOR2 TX_MONITOR1_2
				attr 12: rssi value: 86.25 dB
				attr 13: sampling_frequency value: 560000
				attr 14: sampling_frequency_available value: [520833 1 30720000]
			voltage3:  (output)
			8 channel-specific attributes found:
				attr  0: filter_fir_en value: 1
				attr  1: raw value: 306
				attr  2: rf_bandwidth value: 200000
				attr  3: rf_bandwidth_available value: [200000 1 40000000]
				attr  4: rf_port_select_available value: A B
				attr  5: sampling_frequency value: 560000
				attr  6: sampling_frequency_available value: [520833 1 30720000]
				attr  7: scale value: 1.000000
			altvoltage0: RX_LO (output)
			8 channel-specific attributes found:
				attr  0: external value: 0
				attr  1: fastlock_load value: 0
				attr  2: fastlock_recall ERROR: Invalid argument (22)
				attr  3: fastlock_save value: 0 254,254,254,254,254,254,254,254,254,254,254,254,254,254,254,254
				attr  4: fastlock_store value: 0
				attr  5: frequency value: 1129749948
				attr  6: frequency_available value: [46875001 1 6000000000]
				attr  7: powerdown value: 0
			voltage2:  (output)
			8 channel-specific attributes found:
				attr  0: filter_fir_en value: 1
				attr  1: raw value: 306
				attr  2: rf_bandwidth value: 200000
				attr  3: rf_bandwidth_available value: [200000 1 40000000]
				attr  4: rf_port_select_available value: A B
				attr  5: sampling_frequency value: 560000
				attr  6: sampling_frequency_available value: [520833 1 30720000]
				attr  7: scale value: 1.000000
			temp0:  (input)
			1 channel-specific attributes found:
				attr  0: input value: 40351
			voltage0:  (output)
			10 channel-specific attributes found:
				attr  0: filter_fir_en value: 1
				attr  1: hardwaregain value: 0.000000 dB
				attr  2: hardwaregain_available value: [-89.750000 0.250000 0.000000]
				attr  3: rf_bandwidth value: 200000
				attr  4: rf_bandwidth_available value: [200000 1 40000000]
				attr  5: rf_port_select value: A
				attr  6: rf_port_select_available value: A B
				attr  7: rssi value: 0.00 dB
				attr  8: sampling_frequency value: 560000
				attr  9: sampling_frequency_available value: [520833 1 30720000]
			voltage2:  (input)
			13 channel-specific attributes found:
				attr  0: bb_dc_offset_tracking_en value: 1
				attr  1: filter_fir_en value: 1
				attr  2: gain_control_mode_available value: manual fast_attack slow_attack hybrid
				attr  3: offset value: 57
				attr  4: quadrature_tracking_en value: 1
				attr  5: raw value: 957
				attr  6: rf_bandwidth value: 1000000
				attr  7: rf_bandwidth_available value: [200000 1 56000000]
				attr  8: rf_dc_offset_tracking_en value: 1
				attr  9: rf_port_select_available value: A_BALANCED B_BALANCED C_BALANCED A_N A_P B_N B_P C_N C_P TX_MONITOR1 TX_MONITOR2 TX_MONITOR1_2
				attr 10: sampling_frequency value: 560000
				attr 11: sampling_frequency_available value: [520833 1 30720000]
				attr 12: scale value: 0.305250
			out:  (input, WARN:iio_channel_get_type()=UNKNOWN)
			1 channel-specific attributes found:
				attr  0: voltage_filter_fir_en value: 1
		18 device-specific attributes found:
				attr  0: calib_mode value: manual_tx_quad 26
				attr  1: calib_mode_available value: auto manual manual_tx_quad tx_quad rf_dc_offs rssi_gain_step
				attr  2: dcxo_tune_coarse ERROR: No such device (19)
				attr  3: dcxo_tune_coarse_available value: [0 0 0]
				attr  4: dcxo_tune_fine ERROR: No such device (19)
				attr  5: dcxo_tune_fine_available value: [0 0 0]
				attr  6: ensm_mode value: fdd
				attr  7: ensm_mode_available value: sleep wait alert fdd pinctrl pinctrl_fdd_indep
				attr  8: filter_fir_config value: FIR Rx: 128,4 Tx: 128,4
				attr  9: gain_table_config value: <gaintable AD9361 type=FULL dest=3 start=0 end=1300000000>
-1, 0x00, 0x00, 0x20
-1, 0x00, 0x00, 0x00
-1, 0x00, 0x00, 0x00
0, 0x00, 0x01, 0x00
1, 0x00, 0x02, 0x00
2, 0x00, 0x03, 0x00
3, 0x00, 0x04, 0x00
4, 0x00, 0x05, 0x00
5, 0x01, 0x03, 0x20
6, 0x01, 0x04, 0x00
7, 0x01, 0x05, 0x00
8, 0x01, 0x06, 0x00
9, 0x01, 0x07, 0x00
10, 0x01, 0x08, 0x00
11, 0x01, 0x09, 0x00
12, 0x01, 0x0A, 0x00
13, 0x01, 0x0B, 0x00
14, 0x01, 0x0C, 0x00
15, 0x01, 0x0D, 0x00
16, 0x01, 0x0E, 0x00
17, 0x02, 0x09, 0x20
18, 0x02, 0x0A, 0x00
19, 0x02, 0x0B, 0x00
20, 0x02, 0x0C, 0x00
21, 0x02, 0x0D, 0x00
22, 0x02, 0x0E, 0x00
23, 0x02, 0x0F, 0x00
24, 0x02, 0x10, 0x00
25, 0x02, 0x2B, 0x20
26, 0x02, 0x2C, 0x00
27, 0x04, 0x28, 0x20
28, 0x04, 0x29, 0x00
29, 0x04, 0x2A, 0x00
30, 0x04, 0x2B, 0x00
31, 0x24, 0x20, 0x20
32, 0x24, 0x21, 0x00
33, 0x44, 0x20, 0x20
34, 0x44, 0x21, 0x00
35, 0x44, 0x22, 0x00
36, 0x44, 0x23, 0x00
37, 0x44, 0x24, 0x00
38, 0x44, 0x25, 0x00
39, 0x44, 0x26, 0x00
40, 0x44, 0x27, 0x00
41, 0x44, 0x28, 0x00
42, 0x44, 0x29, 0x00
43, 0x44, 0x2A, 0x00
44, 0x44, 0x2B, 0x00
45, 0x44, 0x2C, 0x00
46, 0x44, 0x2D, 0x00
47, 0x44, 0x2E, 0x00
48, 0x44, 0x2F, 0x00
49, 0x44, 0x30, 0x00
50, 0x44, 0x31, 0x00
51, 0x44, 0x32, 0x00
52, 0x64, 0x2E, 0x20
53, 0x64, 0x2F, 0x00
54, 0x64, 0x30, 0x00
55, 0x64, 0x31, 0x00
56, 0x64, 0x32, 0x00
57, 0x64, 0x33, 0x00
58, 0x64, 0x34, 0x00
59, 0x64, 0x35, 0x00
60, 0x64, 0x36, 0x00
61, 0x64, 0x37, 0x00
62, 0x64, 0x38, 0x00
63, 0x65, 0x38, 0x20
64, 0x66, 0x38, 0x20
65, 0x67, 0x38, 0x20
66, 0x68, 0x38, 0x20
67, 0x69, 0x38, 0x20
68, 0x6A, 0x38, 0x20
69, 0x6B, 0x38, 0x20
70, 0x6C, 0x38, 0x20
71, 0x6D, 0x38, 0x20
72, 0x6E, 0x38, 0x20
73, 0x6F, 0x38, 0x20
</gaintable>
				attr 10: multichip_sync ERROR: Permission denied (13)
				attr 11: rssi_gain_step_error value: lna_error: 0 0 0 0
mixer_error: 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
gain_step_calib_reg_val: 0 0 0 0 0
				attr 12: rx_path_rates value: BBPLL:860160003 ADC:26880000 R2:8960000 R1:4480000 RF:2240000 RXSAMP:560000
				attr 13: trx_rate_governor value: nominal
				attr 14: trx_rate_governor_available value: nominal highest_osr
				attr 15: tx_path_rates value: BBPLL:860160003 DAC:26880000 T2:8960000 T1:4480000 TF:2240000 TXSAMP:560000
				attr 16: xo_correction value: 40000000
				attr 17: xo_correction_available value: [39992000 1 40008000]
		182 debug attributes found:
				debug attr  0: digital_tune value: 0
				debug attr  1: calibration_switch_control value: 0
				debug attr  2: multichip_sync value: 0
				debug attr  3: gaininfo_rx2 ERROR: Resource temporarily unavailable (11)
				debug attr  4: gaininfo_rx1 value: 66 69 0 0 0 0 0 0
				debug attr  5: bist_timing_analysis value: 0
				debug attr  6: gpo_set value: 0
				debug attr  7: bist_tone value: 0
				debug attr  8: bist_prbs value: 0
				debug attr  9: loopback value: 0
				debug attr 10: initialize value: 0
				debug attr 11: adi,bb-clk-change-dig-tune-enable value: 0
				debug attr 12: adi,axi-half-dac-rate-enable value: 0
				debug attr 13: adi,txmon-2-lo-cm value: 48
				debug attr 14: adi,txmon-1-lo-cm value: 48
				debug attr 15: adi,txmon-2-front-end-gain value: 2
				debug attr 16: adi,txmon-1-front-end-gain value: 2
				debug attr 17: adi,txmon-duration value: 8192
				debug attr 18: adi,txmon-delay value: 511
				debug attr 19: adi,txmon-one-shot-mode-enable value: 0
				debug attr 20: adi,txmon-dc-tracking-enable value: 0
				debug attr 21: adi,txmon-high-gain value: 24
				debug attr 22: adi,txmon-low-gain value: 0
				debug attr 23: adi,txmon-low-high-thresh value: 37000
				debug attr 24: adi,gpo3-tx-delay-us value: 0
				debug attr 25: adi,gpo3-rx-delay-us value: 0
				debug attr 26: adi,gpo2-tx-delay-us value: 0
				debug attr 27: adi,gpo2-rx-delay-us value: 0
				debug attr 28: adi,gpo1-tx-delay-us value: 0
				debug attr 29: adi,gpo1-rx-delay-us value: 0
				debug attr 30: adi,gpo0-tx-delay-us value: 0
				debug attr 31: adi,gpo0-rx-delay-us value: 0
				debug attr 32: adi,gpo3-slave-tx-enable value: 0
				debug attr 33: adi,gpo3-slave-rx-enable value: 0
				debug attr 34: adi,gpo2-slave-tx-enable value: 0
				debug attr 35: adi,gpo2-slave-rx-enable value: 0
				debug attr 36: adi,gpo1-slave-tx-enable value: 0
				debug attr 37: adi,gpo1-slave-rx-enable value: 0
				debug attr 38: adi,gpo0-slave-tx-enable value: 0
				debug attr 39: adi,gpo0-slave-rx-enable value: 0
				debug attr 40: adi,gpo3-inactive-state-high-enable value: 0
				debug attr 41: adi,gpo2-inactive-state-high-enable value: 0
				debug attr 42: adi,gpo1-inactive-state-high-enable value: 0
				debug attr 43: adi,gpo0-inactive-state-high-enable value: 0
				debug attr 44: adi,gpo-manual-mode-enable-mask value: 0
				debug attr 45: adi,gpo-manual-mode-enable value: 0
				debug attr 46: adi,aux-dac2-tx-delay-us value: 0
				debug attr 47: adi,aux-dac2-rx-delay-us value: 0
				debug attr 48: adi,aux-dac2-active-in-alert-enable value: 0
				debug attr 49: adi,aux-dac2-active-in-tx-enable value: 0
				debug attr 50: adi,aux-dac2-active-in-rx-enable value: 0
				debug attr 51: adi,aux-dac2-default-value-mV value: 0
				debug attr 52: adi,aux-dac1-tx-delay-us value: 0
				debug attr 53: adi,aux-dac1-rx-delay-us value: 0
				debug attr 54: adi,aux-dac1-active-in-alert-enable value: 0
				debug attr 55: adi,aux-dac1-active-in-tx-enable value: 0
				debug attr 56: adi,aux-dac1-active-in-rx-enable value: 0
				debug attr 57: adi,aux-dac1-default-value-mV value: 0
				debug attr 58: adi,aux-dac-manual-mode-enable value: 1
				debug attr 59: adi,aux-adc-decimation value: 256
				debug attr 60: adi,aux-adc-rate value: 40000000
				debug attr 61: adi,temp-sense-decimation value: 256
				debug attr 62: adi,temp-sense-periodic-measurement-enable value: 1
				debug attr 63: adi,temp-sense-offset-signed value: 206
				debug attr 64: adi,temp-sense-measurement-interval-ms value: 1000
				debug attr 65: adi,elna-gaintable-all-index-enable value: 0
				debug attr 66: adi,elna-rx2-gpo1-control-enable value: 0
				debug attr 67: adi,elna-rx1-gpo0-control-enable value: 0
				debug attr 68: adi,elna-bypass-loss-mdB value: 0
				debug attr 69: adi,elna-gain-mdB value: 0
				debug attr 70: adi,elna-settling-delay-ns value: 0
				debug attr 71: adi,ctrl-outs-enable-mask value: 255
				debug attr 72: adi,ctrl-outs-index value: 0
				debug attr 73: adi,rssi-duration value: 1000
				debug attr 74: adi,rssi-wait value: 1
				debug attr 75: adi,rssi-delay value: 1
				debug attr 76: adi,rssi-unit-is-rx-samples-enable value: 0
				debug attr 77: adi,rssi-restart-mode value: 3
				debug attr 78: adi,fagc-adc-large-overload-inc-steps value: 2
				debug attr 79: adi,fagc-power-measurement-duration-in-state5 value: 64
				debug attr 80: adi,fagc-rst-gla-if-en-agc-pulled-high-mode value: 0
				debug attr 81: adi,fagc-rst-gla-en-agc-pulled-high-enable value: 0
				debug attr 82: adi,fagc-rst-gla-large-lmt-overload-enable value: 1
				debug attr 83: adi,fagc-rst-gla-large-adc-overload-enable value: 1
				debug attr 84: adi,fagc-energy-lost-stronger-sig-gain-lock-exit-cnt value: 8
				debug attr 85: adi,fagc-rst-gla-engergy-lost-sig-thresh-below-ll value: 10
				debug attr 86: adi,fagc-rst-gla-engergy-lost-goto-optim-gain-enable value: 1
				debug attr 87: adi,fagc-rst-gla-engergy-lost-sig-thresh-exceeded-enable value: 1
				debug attr 88: adi,fagc-rst-gla-stronger-sig-thresh-above-ll value: 10
				debug attr 89: adi,fagc-optimized-gain-offset value: 5
				debug attr 90: adi,fagc-rst-gla-stronger-sig-thresh-exceeded-enable value: 1
				debug attr 91: adi,fagc-use-last-lock-level-for-set-gain-enable value: 1
				debug attr 92: adi,fagc-gain-index-type-after-exit-rx-mode value: 0
				debug attr 93: adi,fagc-gain-increase-after-gain-lock-enable value: 0
				debug attr 94: adi,fagc-final-overrange-count value: 3
				debug attr 95: adi,fagc-lmt-final-settling-steps value: 1
				debug attr 96: adi,fagc-lpf-final-settling-steps value: 1
				debug attr 97: adi,fagc-lock-level-gain-increase-upper-limit value: 5
				debug attr 98: adi,fagc-lock-level-lmt-gain-increase-enable value: 1
				debug attr 99: adi,fagc-lp-thresh-increment-steps value: 1
				debug attr 100: adi,fagc-lp-thresh-increment-time value: 5
				debug attr 101: adi,fagc-allow-agc-gain-increase-enable value: 0
				debug attr 102: adi,fagc-state-wait-time-ns value: 260
				debug attr 103: adi,fagc-dec-pow-measurement-duration value: 64
				debug attr 104: adi,agc-immed-gain-change-if-large-lmt-overload-enable value: 0
				debug attr 105: adi,agc-immed-gain-change-if-large-adc-overload-enable value: 0
				debug attr 106: adi,agc-gain-update-interval-us value: 1000
				debug attr 107: adi,agc-sync-for-gain-counter-enable value: 0
				debug attr 108: adi,agc-dig-gain-step-size value: 4
				debug attr 109: adi,agc-dig-saturation-exceed-counter value: 3
				debug attr 110: adi,agc-lmt-overload-large-inc-steps value: 2
				debug attr 111: adi,agc-lmt-overload-small-exceed-counter value: 10
				debug attr 112: adi,agc-lmt-overload-large-exceed-counter value: 10
				debug attr 113: adi,agc-adc-lmt-small-overload-prevent-gain-inc-enable value: 0
				debug attr 114: adi,agc-adc-large-overload-inc-steps value: 2
				debug attr 115: adi,agc-adc-large-overload-exceed-counter value: 10
				debug attr 116: adi,agc-adc-small-overload-exceed-counter value: 10
				debug attr 117: adi,agc-outer-thresh-low-inc-steps value: 2
				debug attr 118: adi,agc-outer-thresh-low value: 18
				debug attr 119: adi,agc-inner-thresh-low-inc-steps value: 1
				debug attr 120: adi,agc-inner-thresh-low value: 12
				debug attr 121: adi,agc-inner-thresh-high-dec-steps value: 1
				debug attr 122: adi,agc-inner-thresh-high value: 10
				debug attr 123: adi,agc-outer-thresh-high-dec-steps value: 2
				debug attr 124: adi,agc-outer-thresh-high value: 5
				debug attr 125: adi,agc-attack-delay-extra-margin-us value: 1
				debug attr 126: adi,mgc-split-table-ctrl-inp-gain-mode value: 0
				debug attr 127: adi,mgc-dec-gain-step value: 2
				debug attr 128: adi,mgc-inc-gain-step value: 2
				debug attr 129: adi,mgc-rx2-ctrl-inp-enable value: 0
				debug attr 130: adi,mgc-rx1-ctrl-inp-enable value: 0
				debug attr 131: adi,gc-use-rx-fir-out-for-dec-pwr-meas-enable value: 0
				debug attr 132: adi,gc-max-dig-gain value: 15
				debug attr 133: adi,gc-dig-gain-enable value: 0
				debug attr 134: adi,gc-low-power-thresh value: 24
				debug attr 135: adi,gc-dec-pow-measurement-duration value: 8192
				debug attr 136: adi,gc-lmt-overload-low-thresh value: 704
				debug attr 137: adi,gc-lmt-overload-high-thresh value: 800
				debug attr 138: adi,gc-adc-large-overload-thresh value: 58
				debug attr 139: adi,gc-adc-small-overload-thresh value: 47
				debug attr 140: adi,gc-adc-ovr-sample-size value: 4
				debug attr 141: adi,gc-rx2-mode value: 2
				debug attr 142: adi,gc-rx1-mode value: 2
				debug attr 143: adi,update-tx-gain-in-alert-enable value: 0
				debug attr 144: adi,tx-attenuation-mdB value: 10000
				debug attr 145: adi,rf-tx-bandwidth-hz value: 18000000
				debug attr 146: adi,rf-rx-bandwidth-hz value: 18000000
				debug attr 147: adi,qec-tracking-slow-mode-enable value: 0
				debug attr 148: adi,dc-offset-count-low-range value: 50
				debug attr 149: adi,dc-offset-count-high-range value: 40
				debug attr 150: adi,dc-offset-attenuation-low-range value: 5
				debug attr 151: adi,dc-offset-attenuation-high-range value: 6
				debug attr 152: adi,dc-offset-tracking-update-event-mask value: 5
				debug attr 153: adi,clk-output-mode-select value: 0
				debug attr 154: adi,external-rx-lo-enable value: 0
				debug attr 155: adi,external-tx-lo-enable value: 0
				debug attr 156: adi,xo-disable-use-ext-refclk-enable value: 1
				debug attr 157: adi,tx-lo-powerdown-managed-enable value: 1
				debug attr 158: adi,trx-synthesizer-target-fref-overwrite-hz value: 80008000
				debug attr 159: adi,rx1-rx2-phase-inversion-enable value: 0
				debug attr 160: adi,tx-rf-port-input-select-lock-enable value: 1
				debug attr 161: adi,rx-rf-port-input-select-lock-enable value: 1
				debug attr 162: adi,tx-rf-port-input-select value: 0
				debug attr 163: adi,rx-rf-port-input-select value: 0
				debug attr 164: adi,split-gain-table-mode-enable value: 0
				debug attr 165: adi,1rx-1tx-mode-use-tx-num value: 1
				debug attr 166: adi,1rx-1tx-mode-use-rx-num value: 1
				debug attr 167: adi,2rx-2tx-mode-enable value: 0
				debug attr 168: adi,digital-interface-tune-fir-disable value: 1
				debug attr 169: adi,digital-interface-tune-skip-mode value: 0
				debug attr 170: adi,tx-fastlock-pincontrol-enable value: 0
				debug attr 171: adi,rx-fastlock-pincontrol-enable value: 0
				debug attr 172: adi,rx-fastlock-delay-ns value: 0
				debug attr 173: adi,tx-fastlock-delay-ns value: 0
				debug attr 174: adi,tdd-skip-vco-cal-enable value: 0
				debug attr 175: adi,tdd-use-dual-synth-mode-enable value: 0
				debug attr 176: adi,debug-mode-enable value: 0
				debug attr 177: adi,ensm-enable-txnrx-control-enable value: 0
				debug attr 178: adi,ensm-enable-pin-pulse-mode-enable value: 0
				debug attr 179: adi,frequency-division-duplex-independent-mode-enable value: 0
				debug attr 180: adi,frequency-division-duplex-mode-enable value: 1
				debug attr 181: direct_reg_access value: 0x50
		No trigger on this device
	iio:device2: xadc
		10 channels found:
			voltage5: vccoddr (input)
			2 channel-specific attributes found:
				attr  0: raw value: 1832
				attr  1: scale value: 0.732421875
			voltage0: vccint (input)
			2 channel-specific attributes found:
				attr  0: raw value: 1372
				attr  1: scale value: 0.732421875
			voltage4: vccpaux (input)
			2 channel-specific attributes found:
				attr  0: raw value: 2441
				attr  1: scale value: 0.732421875
			temp0:  (input)
			3 channel-specific attributes found:
				attr  0: offset value: -2219
				attr  1: raw value: 2692
				attr  2: scale value: 123.040771484
			voltage7: vrefn (input)
			2 channel-specific attributes found:
				attr  0: raw value: -7
				attr  1: scale value: 0.732421875
			voltage1: vccaux (input)
			2 channel-specific attributes found:
				attr  0: raw value: 2441
				attr  1: scale value: 0.732421875
			voltage2: vccbram (input)
			2 channel-specific attributes found:
				attr  0: raw value: 1368
				attr  1: scale value: 0.732421875
			voltage3: vccpint (input)
			2 channel-specific attributes found:
				attr  0: raw value: 1365
				attr  1: scale value: 0.732421875
			voltage8:  (input)
			2 channel-specific attributes found:
				attr  0: raw value: 3675
				attr  1: scale value: 0.244140625
			voltage6: vrefp (input)
			2 channel-specific attributes found:
				attr  0: raw value: 1694
				attr  1: scale value: 0.732421875
		1 device-specific attributes found:
				attr  0: sampling_frequency value: 961538
		No trigger on this device
	iio:device3: cf-ad9361-dds-core-lpc (buffer capable)
		12 channels found:
			voltage0:  (output, index: 0, format: le:S16/16>>0)
			4 channel-specific attributes found:
				attr  0: calibphase value: 0.000000
				attr  1: calibscale value: 1.000000
				attr  2: sampling_frequency value: 560000
				attr  3: sampling_frequency_available value: 560000 70000 
			voltage1:  (output, index: 1, format: le:S16/16>>0)
			4 channel-specific attributes found:
				attr  0: calibphase value: 0.000000
				attr  1: calibscale value: 1.000000
				attr  2: sampling_frequency value: 560000
				attr  3: sampling_frequency_available value: 560000 70000 
			voltage2:  (output, index: 2, format: le:S16/16>>0)
			4 channel-specific attributes found:
				attr  0: calibphase value: 0.000000
				attr  1: calibscale value: 1.000000
				attr  2: sampling_frequency value: 560000
				attr  3: sampling_frequency_available value: 560000 70000 
			voltage3:  (output, index: 3, format: le:S16/16>>0)
			4 channel-specific attributes found:
				attr  0: calibphase value: 0.000000
				attr  1: calibscale value: 1.000000
				attr  2: sampling_frequency value: 560000
				attr  3: sampling_frequency_available value: 560000 70000 
			altvoltage3: TX1_Q_F2 (output)
			5 channel-specific attributes found:
				attr  0: frequency value: 239389
				attr  1: phase value: 0
				attr  2: raw value: 1
				attr  3: sampling_frequency value: 560000
				attr  4: scale value: 0.000000
			altvoltage1: TX1_I_F2 (output)
			5 channel-specific attributes found:
				attr  0: frequency value: 239389
				attr  1: phase value: 90000
				attr  2: raw value: 1
				attr  3: sampling_frequency value: 560000
				attr  4: scale value: 0.000000
			altvoltage0: TX1_I_F1 (output)
			5 channel-specific attributes found:
				attr  0: frequency value: 239389
				attr  1: phase value: 90000
				attr  2: raw value: 1
				attr  3: sampling_frequency value: 560000
				attr  4: scale value: 0.000000
			altvoltage7: TX2_Q_F2 (output)
			5 channel-specific attributes found:
				attr  0: frequency value: 239389
				attr  1: phase value: 0
				attr  2: raw value: 1
				attr  3: sampling_frequency value: 560000
				attr  4: scale value: 0.000000
			altvoltage6: TX2_Q_F1 (output)
			5 channel-specific attributes found:
				attr  0: frequency value: 239389
				attr  1: phase value: 0
				attr  2: raw value: 1
				attr  3: sampling_frequency value: 560000
				attr  4: scale value: 0.000000
			altvoltage5: TX2_I_F2 (output)
			5 channel-specific attributes found:
				attr  0: frequency value: 239389
				attr  1: phase value: 90000
				attr  2: raw value: 1
				attr  3: sampling_frequency value: 560000
				attr  4: scale value: 0.000000
			altvoltage2: TX1_Q_F1 (output)
			5 channel-specific attributes found:
				attr  0: frequency value: 239389
				attr  1: phase value: 0
				attr  2: raw value: 1
				attr  3: sampling_frequency value: 560000
				attr  4: scale value: 0.000000
			altvoltage4: TX2_I_F1 (output)
			5 channel-specific attributes found:
				attr  0: frequency value: 239389
				attr  1: phase value: 90000
				attr  2: raw value: 1
				attr  3: sampling_frequency value: 560000
				attr  4: scale value: 0.000000
		3 buffer-specific attributes found:
				attr  0: data_available value: 0
				attr  1: length_align_bytes value: 8
				attr  2: watermark value: 2048
		1 debug attributes found:
				debug attr  0: direct_reg_access value: 0x90162
		No trigger on this device
	iio:device4: cf-ad9361-lpc (buffer capable)
		2 channels found:
			voltage0:  (input, index: 0, format: le:S12/16>>0)
			6 channel-specific attributes found:
				attr  0: calibbias value: 0
				attr  1: calibphase value: 0.000000
				attr  2: calibscale value: 1.000000
				attr  3: samples_pps ERROR: No such device (19)
				attr  4: sampling_frequency value: 560000
				attr  5: sampling_frequency_available value: 560000 70000 
			voltage1:  (input, index: 1, format: le:S12/16>>0)
			6 channel-specific attributes found:
				attr  0: calibbias value: 0
				attr  1: calibphase value: 0.000000
				attr  2: calibscale value: 1.000000
				attr  3: samples_pps ERROR: No such device (19)
				attr  4: sampling_frequency value: 560000
				attr  5: sampling_frequency_available value: 560000 70000 
		3 buffer-specific attributes found:
				attr  0: data_available value: 0
				attr  1: length_align_bytes value: 8
				attr  2: watermark value: 2048
		2 debug attributes found:
				debug attr  0: pseudorandom_err_check value: CH0 : PN9 : Out of Sync : PN Error
CH1 : PN9 : Out of Sync : PN Error
				debug attr  1: direct_reg_access value: 0x0
		No trigger on this device
