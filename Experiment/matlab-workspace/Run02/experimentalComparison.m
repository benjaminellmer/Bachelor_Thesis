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

dataNone_01 = readmatrix("results-1000-None_01.csv","DecimalSeparator",",","Delimiter"," ");
dataNone_02 = readmatrix("results-1000-None_02.csv","DecimalSeparator",",","Delimiter"," ");
dataNone_03 = readmatrix("results-1000-None_03.csv","DecimalSeparator",",","Delimiter"," ");
dataNone_04 = readmatrix("results-1000-None_04.csv","DecimalSeparator",",","Delimiter"," ");
dataNone_05 = readmatrix("results-1000-None_05.csv","DecimalSeparator",",","Delimiter"," ");

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

noneAvg_01 = mean(dataNone_01);
noneAvg_02 = mean(dataNone_02);
noneAvg_03 = mean(dataNone_03);
noneAvg_04 = mean(dataNone_04);
noneAvg_05 = mean(dataNone_05);
total_none = (noneAvg_01 + noneAvg_02 + noneAvg_03 + noneAvg_04 + noneAvg_05) / 5;
first_none = (dataNone_01(1) + dataNone_02(1) + dataNone_03(1) + dataNone_04(1) + dataNone_05(1)) / 5;

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


jwt_recreate_All = [rot90(dataJWT_recreate_01); rot90(dataJWT_recreate_02); rot90(dataJWT_recreate_03); rot90(dataJWT_recreate_04); rot90(dataJWT_recreate_05)];
jwt_reuse_All = [rot90(dataJWT_reuse_01); rot90(dataJWT_reuse_02); rot90(dataJWT_reuse_03); rot90(dataJWT_reuse_04); rot90(dataJWT_reuse_05)];
mtlsAll = [rot90(dataMTLS_01); rot90(dataMTLS_02); rot90(dataMTLS_03); rot90(dataMTLS_04); rot90(dataMTLS_05)];
noneAll = [rot90(dataNone_01); rot90(dataNone_02); rot90(dataNone_03); rot90(dataNone_04); rot90(dataNone_05)];

means_jwt_reuse = flip(rot90(mean(jwt_reuse_All)),1);
means_jwt_recreate = flip(rot90(mean(jwt_recreate_All)),1);
means_mtls = flip(rot90(mean(mtlsAll)),1);
means_none = flip(rot90(mean(noneAll)),1);

hold on
plot(smooth(means_jwt_recreate,10));
plot(smooth(means_mtls,10));
plot(smooth(means_jwt_reuse,10));
plot(smooth(means_none,10));
legend({'\beta JWT','\alpha mTLS','\gamma JWT (reuse)', '\delta only TLS'},'FontSize',12)
xlabel('iteration')
ylabel('average reqeust duration [ms]')
ylim([0 25])
xlim([10 1000])

jwt_recreate_All(:,1) = jwt_recreate_All(:,2);
jwt_reuse_All(:,1) = jwt_reuse_All(:,2);
mtlsAll(:,1) = mtlsAll(:,2);
noneAll(:,1) = noneAll(:,2);

minMtls = min(min(mtlsAll));
min_jwt_reuse = min(min(jwt_reuse_All));
min_jwt_recreate = min(min(jwt_recreate_All));
minNone = min(min(noneAll));

maxMtls = max(max(mtlsAll));
max_jwt_reuse = max(max(jwt_reuse_All));
max_jwt_recreate = max(max(jwt_recreate_All));
maxNone = max(max(noneAll));
