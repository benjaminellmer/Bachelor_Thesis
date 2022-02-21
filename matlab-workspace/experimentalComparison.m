clear

dataJWT_reuse_01 = readmatrix("results-1000-JWT_reuse_01.csv","DecimalSeparator",",","Delimiter"," ");
dataJWT_reuse_02 = readmatrix("results-1000-JWT_reuse_02.csv","DecimalSeparator",",","Delimiter"," ");
dataJWT_reuse_03 = readmatrix("results-1000-JWT_reuse_03.csv","DecimalSeparator",",","Delimiter"," ");
dataJWT_reuse_04 = readmatrix("results-1000-JWT_reuse_04.csv","DecimalSeparator",",","Delimiter"," ");
dataJWT_reuse_05 = readmatrix("results-1000-JWT_reuse_05.csv","DecimalSeparator",",","Delimiter"," ");

dataJWT_recreate_01 = readmatrix("results-1000-JWT_recreate_01.csv","DecimalSeparator",",","Delimiter"," ");
dataJWT_recreate_02 = readmatrix("results-1000-JWT_recreate_02.csv","DecimalSeparator",",","Delimiter"," ");
dataJWT_recreate_03 = readmatrix("results-1000-JWT_recreate_03.csv","DecimalSeparator",",","Delimiter"," ");
dataJWT_recreate_04 = readmatrix("results-1000-JWT_recreate_04.csv","DecimalSeparator",",","Delimiter"," ");
dataJWT_recreate_05 = readmatrix("results-1000-JWT_recreate_05.csv","DecimalSeparator",",","Delimiter"," ");

dataMTLS_01 = readmatrix("results-1000-MTLS_01.csv","DecimalSeparator",",","Delimiter"," ");
dataMTLS_02 = readmatrix("results-1000-MTLS_02.csv","DecimalSeparator",",","Delimiter"," ");
dataMTLS_03 = readmatrix("results-1000-MTLS_03.csv","DecimalSeparator",",","Delimiter"," ");
dataMTLS_04 = readmatrix("results-1000-MTLS_04.csv","DecimalSeparator",",","Delimiter"," ");
dataMTLS_05 = readmatrix("results-1000-MTLS_05.csv","DecimalSeparator",",","Delimiter"," ");

jwtReuseAvg_01 = mean(dataJWT_reuse_01);
jwtReuseAvg_02 = mean(dataJWT_reuse_02);
jwtReuseAvg_03 = mean(dataJWT_reuse_03);
jwtReuseAvg_04 = mean(dataJWT_reuse_04);
jwtReuseAvg_05 = mean(dataJWT_reuse_05);
total_jwt_reuse = (jwtReuseAvg_01 + jwtReuseAvg_02 + jwtReuseAvg_03 + jwtReuseAvg_04 + jwtReuseAvg_05) / 5;
first_jwt_reuse = (dataJWT_reuse_01(1) + dataJWT_reuse_02(1) + dataJWT_reuse_03(1) + dataJWT_reuse_04(1) + dataJWT_reuse_05(1)) / 5;

jwtRecreateAvg_01 = mean(dataJWT_recreate_01);
jwtRecreateAvg_02 = mean(dataJWT_recreate_02);
jwtRecreateAvg_03 = mean(dataJWT_recreate_03);
jwtRecreateAvg_04 = mean(dataJWT_recreate_04);
jwtRecreateAvg_05 = mean(dataJWT_recreate_05);
total_jwt_recreate = (jwtRecreateAvg_01 + jwtRecreateAvg_02 + jwtRecreateAvg_03 + jwtRecreateAvg_04 + jwtRecreateAvg_05) / 5;
first_jwt_recreate = (dataJWT_recreate_01(1) + dataJWT_recreate_02(1) + dataJWT_recreate_03(1) + dataJWT_recreate_04(1) + dataJWT_recreate_05(1)) / 5;

mtlsAvg_01 = mean(dataMTLS_01);
mtlsAvg_02 = mean(dataMTLS_02);
mtlsAvg_03 = mean(dataMTLS_03);
mtlsAvg_04 = mean(dataMTLS_04);
mtlsAvg_05 = mean(dataMTLS_05);
total_mtls = (mtlsAvg_01 + mtlsAvg_02 + mtlsAvg_03 + mtlsAvg_04 + mtlsAvg_05) / 5;
first_mtls = (dataMTLS_01(1) + dataMTLS_02(1) + dataMTLS_03(1) + dataMTLS_04(1) + dataMTLS_05(1)) / 5;

%noneAvg_01 = mean(dataNone_01);
%noneAvg_02 = mean(dataNone_02);
%noneAvg_03 = mean(dataNone_03);
%noneTotalAvg = (noneAvg_01 + noneAvg_02 + noneAvg_03) / 3;
%
%mtlsAvg_01 = mean(dataMTLS_01);
%mtlsAvg_02 = mean(dataMTLS_02);
%mtlsAvg_03 = mean(dataMTLS_03);
%mtlsTotalAvg = (mtlsAvg_01 + mtlsAvg_02 + mtlsAvg_03) / 3;
%
%jwtAvg_01 = mean(dataJWT_01);
%jwtAvg_02 = mean(dataJWT_02);
%jwtAvg_03 = mean(dataJWT_03);
%jwtTotalAvg = (jwtAvg_01 + jwtAvg_02 + jwtAvg_03) / 3;

windowSize = 30;
a = 1;
b = (1/windowSize)*ones(1,windowSize);

y1 = filter(b,a,dataMTLS_01(2:1000));
y2 = filter(b,a,dataJWT_recreate_02(2:1000));
y3 = filter(b,a,dataJWT_reuse_01(2:1000));

hold on

plot(smooth(y1(30:999)));
plot(smooth(y2(30:999)));
plot(smooth(y3(30:999)));
legend('mtls','jwt recreate','jwt reuse')
ylim([0 20])
%hold on
%plot(smooth(dataMTLS_01(10:1000),100));